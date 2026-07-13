using System;

namespace TripleMatch.Presentation.Gameplay
{
    public interface IBoardService
    {
        // Fired only for picks the board actually allows (topmost layer at that
        // position) - raw taps on covered items never reach this event.
        event Action<ItemView> ItemPickValidated;
    }
}
