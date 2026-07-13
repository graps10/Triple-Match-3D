using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TripleMatch.Presentation.UI
{
    public class PopupView : ScreenView
    {
        [SerializeField] private Button closeButton;

        public event Action CloseRequested;

        [Inject]
        public void Construct(PopupPresenter presenter)
        {
            presenter.Bind(this);
            Presenter = presenter;
            closeButton.onClick.AddListener(() => CloseRequested?.Invoke());
        }
    }
}
