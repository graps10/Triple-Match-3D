using System.Collections.Generic;

namespace TripleMatch.Domain
{
    public class Tray
    {
        public const int CAPACITY = 7;

        private readonly List<ItemType> _slots = new();

        public IReadOnlyList<ItemType> Slots => _slots;
        public bool IsFull => _slots.Count >= CAPACITY;

        // Inserts the item next to the last matching type (grouping identical items
        // together), or appends it at the end if no match exists yet. Returns the
        // slot index it landed on, or -1 if the tray was already full.
        public int Collect(ItemType type)
        {
            if (IsFull)
                return -1;

            int lastMatchIndex = _slots.LastIndexOf(type);
            int insertIndex = lastMatchIndex >= 0 ? lastMatchIndex + 1 : _slots.Count;

            _slots.Insert(insertIndex, type);
            return insertIndex;
        }

        // Removes a contiguous run (e.g. a resolved match), shifting later slots left.
        public void RemoveRun(int startIndex, int count) => _slots.RemoveRange(startIndex, count);
    }
}
