namespace TripleMatch.Presentation.Gameplay
{
    public interface ITrayService
    {
        bool IsFull { get; }

        // True only right after a Collect that hasn't since been consumed by a match or
        // followed by another Collect - single-level undo, not a full history.
        bool CanUndo { get; }

        void Collect(ItemView item);
        void Undo();
    }
}
