﻿using System;
using System.Collections.Generic;
using Infastructure;
using Player.States;

namespace Player
{
    public class HeroStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;
        
        public HeroStateMachine()
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(FindEnemyState)] = new FindEnemyState(this),
                [typeof(MovingState)] = new MovingState(this),
                [typeof(ShootingState)] = new ShootingState(this),
            };
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayLoad>(TPayLoad payload) where TState : class, IPayloadedState<TPayLoad>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            
            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        public TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}