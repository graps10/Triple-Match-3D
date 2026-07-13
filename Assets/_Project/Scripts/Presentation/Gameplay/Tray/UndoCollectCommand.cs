using UnityEngine;

namespace TripleMatch.Presentation.Gameplay
{
    // Command pattern: captures everything needed to reverse one Collect - which item,
    // which tray slot it landed in, and where on the board it came from.
    public class UndoCollectCommand
    {
        public ItemView Item { get; }
        public int SlotIndex { get; }
        public Vector2 BoardPosition { get; }
        public int BoardLayer { get; }

        public UndoCollectCommand(ItemView item, int slotIndex, Vector2 boardPosition, int boardLayer)
        {
            Item = item;
            SlotIndex = slotIndex;
            BoardPosition = boardPosition;
            BoardLayer = boardLayer;
        }
    }
}
