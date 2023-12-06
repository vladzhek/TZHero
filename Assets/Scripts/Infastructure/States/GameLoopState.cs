using Player;
using Player.States;

namespace Infastructure
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;
        
        public GameLoopState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            
        }

        public void Exit()
        {
            
        }

        public void Enter()
        {
            
        }
    }
}