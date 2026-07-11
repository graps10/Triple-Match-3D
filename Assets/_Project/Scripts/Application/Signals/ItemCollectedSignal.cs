using TripleMatch.Domain;

namespace TripleMatch.Application.Signals
{
    public class ItemCollectedSignal
    {
        public ItemType ItemType { get; }
        public int SlotIndex { get; }

        public ItemCollectedSignal(ItemType itemType, int slotIndex)
        {
            ItemType = itemType;
            SlotIndex = slotIndex;
        }
    }
}
