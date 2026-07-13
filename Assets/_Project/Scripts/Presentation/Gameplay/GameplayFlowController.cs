using System;
using TripleMatch.Application.Signals;
using TripleMatch.Presentation.UI;
using Zenject;

namespace TripleMatch.Presentation.Gameplay
{
    // Gameplay-scoped bridge from gameplay signals to UI (SignalBus only exists while
    // this scene is loaded, so whatever reacts to these signals has to live here).
    // Actually returning to Meta / retrying now belongs to the popups themselves
    // (WinPopupPresenter/LosePopupPresenter) - this class only decides which popup to show.
    public class GameplayFlowController : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly IScreenService _screenService;

        public GameplayFlowController(SignalBus signalBus, IScreenService screenService)
        {
            _signalBus = signalBus;
            _screenService = screenService;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<LevelRewardSignal>(OnLevelReward);
            _signalBus.Subscribe<GameLostSignal>(OnGameLost);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<LevelRewardSignal>(OnLevelReward);
            _signalBus.Unsubscribe<GameLostSignal>(OnGameLost);
        }

        private void OnLevelReward(LevelRewardSignal signal) =>
            _screenService.Push<WinPopupView>(ScreenId.WinPopup, signal.Stars, signal.Coins);

        private void OnGameLost(GameLostSignal signal) => _screenService.Push<LosePopupView>(ScreenId.LosePopup);
    }
}
