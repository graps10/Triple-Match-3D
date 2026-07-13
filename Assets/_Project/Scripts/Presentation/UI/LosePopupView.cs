using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TripleMatch.Presentation.UI
{
    public class LosePopupView : ScreenView
    {
        [SerializeField] private Button retryButton;

        public event Action RetryRequested;

        [Inject]
        public void Construct(LosePopupPresenter presenter)
        {
            presenter.Bind(this);
            Presenter = presenter;
            retryButton.onClick.AddListener(() => RetryRequested?.Invoke());
        }
    }
}
