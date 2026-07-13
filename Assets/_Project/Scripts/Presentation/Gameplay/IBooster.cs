namespace TripleMatch.Presentation.Gameplay
{
    // Strategy: interchangeable behaviors behind one contract - whoever activates a
    // booster doesn't need to know which concrete algorithm it's running.
    public interface IBooster
    {
        BoosterType Type { get; }
        int Cost { get; }
        bool CanActivate();
        void Activate();
    }
}
