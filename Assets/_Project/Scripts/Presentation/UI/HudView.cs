using System;
using TripleMatch.Presentation.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TripleMatch.Presentation.UI
{
    public class HudView : ScreenView
    {
        [SerializeField] private Button undoButton;
        [SerializeField] private Button shuffleButton;
        [SerializeField] private Button extraSlotsButton;

        public event Action<BoosterType> BoosterClicked;

        // boosterService arrives as a Push() extraArg (see GameplayFlowController) -
        // IBoosterService is Gameplay-scene-scoped, but HUD is spawned through the
        // project-scoped UIRoot/ScreenService, which can't resolve scene-scoped bindings.
        // Passing the already-resolved instance sidesteps that entirely (same fix as the
        // WinPopupPresenter int-args issue from Day 15, just with an interface instead of
        // a primitive).
        [Inject]
        public void Construct(HudPresenter presenter, IBoosterService boosterService)
        {
            presenter.Bind(this, boosterService);
            Presenter = presenter;

            undoButton.onClick.AddListener(() => BoosterClicked?.Invoke(BoosterType.Undo));
            shuffleButton.onClick.AddListener(() => BoosterClicked?.Invoke(BoosterType.Shuffle));
            extraSlotsButton.onClick.AddListener(() => BoosterClicked?.Invoke(BoosterType.ExtraSlots));
        }
    }
}
