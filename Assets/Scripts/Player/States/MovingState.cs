using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Infastructure;
using Infastructure.Services;
using Services;
using UnityEngine;
using Zenject;

namespace Player.States
{
    public class MovingState : IState
    {
        private HeroStateMachine _heroStateMachine;
        private CancellationTokenSource _cts;
        private IInputService _inputService;

        public MovingState(HeroStateMachine heroStateMachine)
        {
            _heroStateMachine = heroStateMachine;
        }

        public void Exit()
        {
            _cts?.Cancel();
        }

        public async void Enter()
        {
            _inputService = Game.InputService;

            _cts = new CancellationTokenSource();
            await CheckMovement(_cts.Token);
        }
        
        private async UniTask CheckMovement(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                
                if (_inputService.Axis.sqrMagnitude < Constants.EPSILON)
                {
                    _heroStateMachine.Enter<FindEnemyState>();
                }
            }
        }
    }
}