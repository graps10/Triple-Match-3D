using System;
using TripleMatch.Application.Services;

namespace TripleMatch.Presentation.UI
{
    public class HudPresenter : IDisposable
    {
        private readonly ILogService _log;

        public HudPresenter(ILogService log)
        {
            _log = log;
        }

        public void Bind(HudView view)
        {
            _log.Info("HudPresenter: bound to view.");
        }

        // Nothing to unsubscribe yet - placeholder so Days 15/18 have the hook already in
        // place once this starts listening to SignalBus for live HUD updates.
        public void Dispose()
        {
        }
    }
}
