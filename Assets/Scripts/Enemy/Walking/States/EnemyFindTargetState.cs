using System.Threading;
using Cysharp.Threading.Tasks;
using Data;
using Infastructure;
using Services;
using UI.Enemy;
using Zenject;

namespace Enemy.Walking
{
    public class EnemyFindTargetState : IPayloadedState<EnemyWalking>
    {
        private EnemyWalkingStateMachine _enemyWalkingStateMachine;
        private StaticDataService _staticDataService;
        private UnitTrackerService _unitTrackerService;
        
        private CancellationTokenSource _cts;
        private EnemyWalking _unit;

        [Inject]
        public void Construct(StaticDataService staticDataService, UnitTrackerService unitTrackerService)
        {
            _staticDataService = staticDataService;
            _unitTrackerService = unitTrackerService;
        }

        public EnemyFindTargetState(EnemyWalkingStateMachine enemyWalkingStateMachine)
        {
            _enemyWalkingStateMachine = enemyWalkingStateMachine;
        }

        public void Exit()
        {
            _cts.Cancel();
        }

        public async void Enter(EnemyWalking unit)
        {
            InjectService.Instance.Inject(this);
            _unit = unit;
            
            _cts = new CancellationTokenSource();
            await FindPlayer(_cts.Token);
        }

        private async UniTask FindPlayer(CancellationToken cts)
        {
            while (!cts.IsCancellationRequested)
            {
                await UniTask.Yield();
                
                if(_unit == null) return;
                    
                var target = _unitTrackerService.GetPlayerTarget(_unit.gameObject.transform.position);
                
                if (target != null && target.Type == UnitType.Player)
                {
                    _unit.SetAgentTarget(target.gameObject);
                    _enemyWalkingStateMachine.Enter<WalkingShootingState, EnemyWalking>(_unit);
                }
            }
        }
    }
}