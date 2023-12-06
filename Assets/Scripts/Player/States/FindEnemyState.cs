using System.Threading;
using Cysharp.Threading.Tasks;
using Data;
using DefaultNamespace;
using Infastructure;
using Infastructure.Services;
using Services;
using UnityEngine;
using Zenject;

namespace Player.States
{
    public class FindEnemyState : IState
    {
        private HeroStateMachine _heroStateMachine;
        
        private CancellationTokenSource _cts;
        private Transform _targer;

        private UnitTrackerService _unitTrackerService;
        private IInputService _inputService;

        private PlayerData _playerDataProgress;
        
        private float _lastShotTime = 0f;

        [Inject]
        public void Construct(UnitTrackerService unitTrackerService,
                StaticDataService staticDataService)
        {
            _unitTrackerService = unitTrackerService;
        }

        public FindEnemyState(HeroStateMachine heroStateMachine)
        {
            _heroStateMachine = heroStateMachine;
        }

        public async void Enter()
        {
            InjectService.Instance.Inject(this);
            _inputService = Game.InputService;

            _targer = null;
            _cts = new CancellationTokenSource();
                        
            await DetectEnemy(_cts.Token);
        }

        public void Exit()
        {
            _cts?.Cancel();
            _targer = null;
        }

        private async UniTask DetectEnemy(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();

                var target = _unitTrackerService.GetEnemyTarget();
                if (target != null && _inputService.Axis.sqrMagnitude < Constants.EPSILON)
                {
                    _heroStateMachine.Enter<ShootingState, GameObject>(target);
                }
                
                if(_inputService.Axis.sqrMagnitude > Constants.EPSILON)
                {
                    _heroStateMachine.Enter<MovingState>();
                    return;
                }
            }
        }
    }
}