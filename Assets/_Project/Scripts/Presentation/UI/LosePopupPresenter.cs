using System;
using Cysharp.Threading.Tasks;
using TripleMatch.Application.StateMachine;
using TripleMatch.Application.StateMachine.States;

namespace TripleMatch.Presentation.UI
{
    public class LosePopupPresenter : IDisposable
    {
        private readonly IScreenService _screenService;
        private readonly IGameStateMachine _stateMachine;
        private LosePopupView _view;

        public LosePopupPresenter(IScreenService screenService, IGameStateMachine stateMachine)
        {
            _screenService = screenService;
            _stateMachine = stateMachine;
        }

        public void Bind(LosePopupView view)
        {
            _view = view;
            view.RetryRequested += OnRetryRequested;
        }

        // Same selected level (ILevelSelectionService untouched) - re-entering
        // GameplayState just reloads it from scratch.
        private void OnRetryRequested()
        {
            _screenService.Pop();
            _stateMachine.Enter<GameplayState>().Forget();
        }

        public void Dispose() => _view.RetryRequested -= OnRetryRequested;
    }
}
