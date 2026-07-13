using System;
using TripleMatch.Application.Services;

namespace TripleMatch.Presentation.UI
{
    public class PopupPresenter : IDisposable
    {
        private readonly ILogService _log;
        private readonly IScreenService _screenService;
        private PopupView _view;

        public PopupPresenter(ILogService log, IScreenService screenService)
        {
            _log = log;
            _screenService = screenService;
        }

        public void Bind(PopupView view)
        {
            _view = view;
            _log.Info("PopupPresenter: bound to view.");
            view.CloseRequested += OnCloseRequested;
        }

        private void OnCloseRequested() => _screenService.Pop();

        public void Dispose() => _view.CloseRequested -= OnCloseRequested;
    }
}
