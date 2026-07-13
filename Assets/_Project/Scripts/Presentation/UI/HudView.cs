using Zenject;

namespace TripleMatch.Presentation.UI
{
    public class HudView : ScreenView
    {
        [Inject]
        public void Construct(HudPresenter presenter)
        {
            presenter.Bind(this);
            Presenter = presenter;
        }
    }
}
