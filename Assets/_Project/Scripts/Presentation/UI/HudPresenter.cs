using System;
using TripleMatch.Application.Services;
using TripleMatch.Presentation.Gameplay;

namespace TripleMatch.Presentation.UI
{
    public class HudPresenter : IDisposable
    {
        private readonly ILogService _log;
        private IBoosterService _boosterService;
        private HudView _view;

        public HudPresenter(ILogService log)
        {
            _log = log;
        }

        public void Bind(HudView view, IBoosterService boosterService)
        {
            _view = view;
            _boosterService = boosterService;
            _log.Info("HudPresenter: bound to view.");
            view.BoosterClicked += OnBoosterClicked;
        }

        private void OnBoosterClicked(BoosterType type) => _boosterService.Activate(type);

        public void Dispose() => _view.BoosterClicked -= OnBoosterClicked;
    }
}
