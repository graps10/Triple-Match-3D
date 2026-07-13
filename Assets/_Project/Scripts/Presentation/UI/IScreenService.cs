namespace TripleMatch.Presentation.UI
{
    public interface IScreenService
    {
        // extraArgs: per-push data the Presenter's constructor needs but can't get from
        // a binding (e.g. how many stars/coins to show) - same mechanism as WithArguments
        // on a binding, just supplied at Push time instead.
        TView Push<TView>(ScreenId id, params object[] extraArgs) where TView : ScreenView;
        void Pop();
    }
}
