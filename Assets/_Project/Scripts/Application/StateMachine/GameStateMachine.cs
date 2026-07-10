using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TripleMatch.Application.StateMachine.States;
using Zenject;

namespace TripleMatch.Application.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _current;

        public GameStateMachine(DiContainer container)
        {
            _states = new Dictionary<Type, IState>
            {
                [typeof(BootState)] = container.Instantiate<BootState>(new object[] { this }),
                [typeof(MetaState)] = container.Instantiate<MetaState>(),
                [typeof(GameplayState)] = container.Instantiate<GameplayState>(),
            };
        }

        public async UniTask Enter<TState>() where TState : IState
        {
            if (_current != null)
                await _current.Exit();

            _current = _states[typeof(TState)];
            await _current.Enter();
        }
    }
}
