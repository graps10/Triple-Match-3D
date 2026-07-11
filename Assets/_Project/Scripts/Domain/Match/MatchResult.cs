namespace TripleMatch.Domain
{
    // Outcome of a match check: either "no match" or the type, starting slot index,
    // and run length the resolver found (length lets callers remove the exact run
    // without hardcoding the match size a second time).
    public readonly struct MatchResult
    {
        public bool Matched { get; }
        public ItemType Type { get; }
        public int SlotIndex { get; }
        public int Length { get; }

        private MatchResult(bool matched, ItemType type, int slotIndex, int length)
        {
            Matched = matched;
            Type = type;
            SlotIndex = slotIndex;
            Length = length;
        }

        public static MatchResult None => new(false, default, -1, 0);
        public static MatchResult Found(ItemType type, int slotIndex, int length) => new(true, type, slotIndex, length);
    }
}
