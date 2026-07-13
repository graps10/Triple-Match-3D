using System;
using Cysharp.Threading.Tasks;
using TripleMatch.Application.Signals;
using TripleMatch.Application.StateMachine;
using TripleMatch.Application.StateMachine.States;
using Zenject;

namespace TripleMatch.Presentation.Gameplay
{
    // Gameplay-scoped half of the Meta<->Gameplay round trip (the other half is Meta's
    // LevelMapPresenter) - SignalBus only exists while this scene is loaded, so whatever
    // reacts to GameWonSignal has to live here, not in the project-scoped state machine.
    public class GameplayFlowController : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly IGameStateMachine _stateMachine;

        public GameplayFlowController(SignalBus signalBus, IGameStateMachine stateMachine)
        {
            _signalBus = signalBus;
            _stateMachine = stateMachine;
        }

        public void Initialize() => _signalBus.Subscribe<GameWonSignal>(OnGameWon);

        public void Dispose() => _signalBus.Unsubscribe<GameWonSignal>(OnGameWon);

        private void OnGameWon(GameWonSignal signal) => _stateMachine.Enter<MetaState>().Forget();
    }
}
