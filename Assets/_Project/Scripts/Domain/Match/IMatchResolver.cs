using System.Collections.Generic;

namespace TripleMatch.Domain
{
    public interface IMatchResolver
    {
        MatchResult Resolve(IReadOnlyList<ItemType> slots);
    }
}
