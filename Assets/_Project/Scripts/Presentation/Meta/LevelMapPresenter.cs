using TripleMatch.Application.Levels;
using Zenject;

namespace TripleMatch.Presentation.Meta
{
    public class LevelMapPresenter : IInitializable
    {
        private readonly LevelButtonView.Factory _factory;
        private readonly LevelMapView _view;
        private readonly ILevelSource _levelSource;

        public LevelMapPresenter(LevelButtonView.Factory factory, LevelMapView view, ILevelSource levelSource)
        {
            _factory = factory;
            _view = view;
            _levelSource = levelSource;
        }

        public void Initialize()
        {
            for (int i = 0; i < _levelSource.Count; i++)
            {
                LevelButtonView button = _factory.Create();
                button.transform.SetParent(_view.ButtonsContainer, false);
                button.SetLevel(i);
            }
        }
    }
}
