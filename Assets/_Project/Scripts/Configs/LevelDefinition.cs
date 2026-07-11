using System;
using System.Collections.Generic;
using TripleMatch.Domain;
using UnityEngine;

namespace TripleMatch.Configs
{
    // One item placement: Position is the X/Z spot on the board's floor, Layer is the
    // stacking depth (0 = bottom, higher = closer to the camera). Kept separate so level
    // designers write "layer 1", not a hand-picked Z offset.
    [Serializable]
    public struct LevelItemEntry
    {
        public ItemType Type;
        public Vector2 Position;
        public int Layer;
    }

    [CreateAssetMenu(fileName = "Level_", menuName = "TripleMatch/Level Definition")]
    public class LevelDefinition : ScriptableObject
    {
        [SerializeField] private List<LevelItemEntry> items;

        public IReadOnlyList<LevelItemEntry> Items => items;
    }
}
