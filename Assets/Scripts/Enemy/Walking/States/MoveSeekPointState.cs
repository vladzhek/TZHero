using Infastructure;
using Services;
using UI.Enemy;
using Zenject;

namespace Enemy.Walking
{
    public class MoveSeekPointState : IPayloadedState<EnemyWalking>
    {
        [Inject] private SeekCoverService _seekCoverService;
        [Inject] private StaticDataService _staticDataService;
        private EnemyWalkingStateMachine _enemyWalkingStateMachine;

        public MoveSeekPointState(EnemyWalkingStateMachine enemyWalkingStateMachine)
        {
            _enemyWalkingStateMachine = enemyWalkingStateMachine;
        }

        public void Enter(EnemyWalking unit)
        {
            InjectService.Instance.Inject(this);
            var points = _staticDataService.LevelData.LevelConfigs[0].CoverPoints;
            var bestPoint = _seekCoverService.GetBestCoverPoint(unit.transform, points);
            unit.SetAgentTarget(bestPoint.gameObject);
            _enemyWalkingStateMachine.Enter<EnemyFindTargetState, EnemyWalking>(unit);
        }

        public void Exit()
        {
            
        }
    }
}