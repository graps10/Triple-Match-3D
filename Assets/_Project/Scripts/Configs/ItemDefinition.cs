using TripleMatch.Domain;
using UnityEngine;

namespace TripleMatch.Configs
{
    [CreateAssetMenu(fileName = "Item_", menuName = "TripleMatch/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [SerializeField] private ItemType type;
        [SerializeField] private Sprite icon;

        public ItemType Type => type;
        public Sprite Icon => icon;
    }
}
