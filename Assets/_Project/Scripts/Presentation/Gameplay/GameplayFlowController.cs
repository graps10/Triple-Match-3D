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
        private readonly IBoosterService _boosterService;

        public GameplayFlowController(SignalBus signalBus, IScreenService screenService, IBoosterService boosterService)
        {
            _signalBus = signalBus;
            _screenService = screenService;
            _boosterService = boosterService;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<LevelRewardSignal>(OnLevelReward);
            _signalBus.Subscribe<GameLostSignal>(OnGameLost);

            // IBoosterService is Gameplay-scoped and can't be resolved once HUD's own
            // Presenter is built through the project-scoped ScreenService - handing it in
            // as an extraArg here is the fix (see HudView's Construct for the full reasoning).
            _screenService.Push<HudView>(ScreenId.Hud, _boosterService);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<LevelRewardSignal>(OnLevelReward);
            _signalBus.Unsubscribe<GameLostSignal>(OnGameLost);

            // HUD lives on the persistent UIRoot, not in this scene - it survives scene
            // teardown unless popped explicitly. This fires on every way Gameplay ends
            // (Win, Lose+Retry, a scene reload), always leaving HUD as the stack's top by
            // then (Win/Lose popups already popped themselves before triggering the
            // scene transition that gets here).
            _screenService.Pop();
        }

        private void OnLevelReward(LevelRewardSignal signal) =>
            _screenService.Push<WinPopupView>(ScreenId.WinPopup, signal.Stars, signal.Coins);

        private void OnGameLost(GameLostSignal signal) => _screenService.Push<LosePopupView>(ScreenId.LosePopup);
    }
}
