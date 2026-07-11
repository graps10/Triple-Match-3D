namespace TripleMatch.Application.Signals
{
    public class BoardBuiltSignal
    {
        public int ItemCount { get; }

        public BoardBuiltSignal(int itemCount)
        {
            ItemCount = itemCount;
        }
    }
}
