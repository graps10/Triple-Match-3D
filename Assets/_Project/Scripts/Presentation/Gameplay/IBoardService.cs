using System;
using UnityEngine;

namespace TripleMatch.Presentation.Gameplay
{
    public interface IBoardService
    {
        // Fired only for picks the board actually allows (topmost layer at that
        // position) - raw taps on covered items never reach this event.
        event Action<ItemView> ItemPickValidated;

        // Undo booster: puts a previously-picked item back at its original spot.
        void ReturnToBoard(ItemView item, Vector2 position, int layer);

        // Shuffle booster: randomizes which position/layer each current board item sits
        // at (the tray is untouched - only what's still on the board gets reshuffled).
        void Shuffle();
    }
}
