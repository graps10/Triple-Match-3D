using System;
using Cysharp.Threading.Tasks;
using TripleMatch.Application.StateMachine;
using TripleMatch.Application.StateMachine.States;

namespace TripleMatch.Presentation.UI
{
    public class WinPopupPresenter : IDisposable
    {
        private readonly IScreenService _screenService;
        private readonly IGameStateMachine _stateMachine;
        private WinPopupView _view;

        public WinPopupPresenter(IScreenService screenService, IGameStateMachine stateMachine)
        {
            _screenService = screenService;
            _stateMachine = stateMachine;
        }

        // stars/coins passed in directly by WinPopupView.Construct - see the comment
        // there for why they can't be constructor-injected here.
        public void Bind(WinPopupView view, int stars, int coins)
        {
            _view = view;
            view.Show(stars, coins);
            view.NextRequested += OnNextRequested;
        }

        private void OnNextRequested()
        {
            _screenService.Pop();
            _stateMachine.Enter<MetaState>().Forget();
        }

        public void Dispose() => _view.NextRequested -= OnNextRequested;
    }
}
