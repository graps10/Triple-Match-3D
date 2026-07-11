using System.Collections.Generic;

namespace TripleMatch.Domain
{
    public class MatchResolver : IMatchResolver
    {
        private const int Match_Size = 3;

        public MatchResult Resolve(IReadOnlyList<ItemType> slots)
        {
            for (int start = 0; start <= slots.Count - Match_Size; start++)
            {
                if (IsRun(slots, start, Match_Size))
                    return MatchResult.Found(slots[start], start, Match_Size);
            }

            return MatchResult.None;
        }

        private static bool IsRun(IReadOnlyList<ItemType> slots, int start, int length)
        {
            ItemType type = slots[start];

            for (int i = start + 1; i < start + length; i++)
            {
                if (slots[i] != type)
                    return false;
            }

            return true;
        }
    }
}
