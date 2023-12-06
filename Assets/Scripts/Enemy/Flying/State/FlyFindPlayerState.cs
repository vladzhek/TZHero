using System.Threading;
using Cysharp.Threading.Tasks;
using Data;
using Enemy.Walking;
using Infastructure;
using Services;
using UI.Enemy.Flying;
using Zenject;

namespace UI.Enemy.State
{
    public class FlyFindPlayerState : IPayloadedState<EnemyFlying>
    {
        private FlyingStateMachine _flyingStateMachine;
        [Inject] private StaticDataService _staticDataService;
        [Inject] private UnitTrackerService _unitTrackerService;
        
        private CancellationTokenSource _cts;
        private EnemyFlying _unit;

        public FlyFindPlayerState(FlyingStateMachine flyingStateMachine)
        {
            _flyingStateMachine = flyingStateMachine;
        }

        public void Exit()
        {
            _cts.Cancel();
        }

        public async void Enter(EnemyFlying unit)
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
                    _flyingStateMachine.Enter<FlyAttackState, EnemyFlying>(_unit);
                }
            }
        }
    }
}