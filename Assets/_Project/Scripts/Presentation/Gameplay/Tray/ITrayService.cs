namespace TripleMatch.Presentation.Gameplay
{
    public interface ITrayService
    {
        bool IsFull { get; }
        void Collect(ItemView item);
    }
}
