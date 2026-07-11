using TripleMatch.Domain;

namespace TripleMatch.Application.Signals
{
    // Fired once MatchResolver finds and resolves a 3-of-a-kind run in the tray.
    public class MatchMadeSignal
    {
        public ItemType ItemType { get; }
        public int SlotIndex { get; }

        public MatchMadeSignal(ItemType itemType, int slotIndex)
        {
            ItemType = itemType;
            SlotIndex = slotIndex;
        }
    }
}
