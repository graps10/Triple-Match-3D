namespace TripleMatch.Presentation.UI
{
    public interface IScreenService
    {
        TView Push<TView>(ScreenId id) where TView : ScreenView;
        void Pop();
    }
}
