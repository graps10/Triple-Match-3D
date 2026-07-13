using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TripleMatch.Presentation.UI
{
    public class WinPopupView : ScreenView
    {
        [SerializeField] private TextMeshProUGUI starsLabel;
        [SerializeField] private TextMeshProUGUI coinsLabel;
        [SerializeField] private Button nextButton;

        public event Action NextRequested;

        // stars/coins land here (not on WinPopupPresenter's constructor) because this
        // View is the direct target of ScreenService's InstantiatePrefabForComponent call -
        // extraArgs only reach that call's own [Inject] members, not a separately-bound
        // Presenter resolved through its own Container.Bind<WinPopupPresenter>().
        [Inject]
        public void Construct(WinPopupPresenter presenter, int stars, int coins)
        {
            presenter.Bind(this, stars, coins);
            Presenter = presenter;
            nextButton.onClick.AddListener(() => NextRequested?.Invoke());
        }

        public void Show(int stars, int coins)
        {
            starsLabel.text = $"{stars} / 3";
            coinsLabel.text = $"+{coins}";
        }
    }
}
