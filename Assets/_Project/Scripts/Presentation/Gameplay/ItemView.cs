using TripleMatch.Configs;
using TripleMatch.Domain;
using UnityEngine;
using Zenject;

namespace TripleMatch.Presentation.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class ItemView : MonoBehaviour
    {
        private ItemDefinition _definition;

        public ItemType Type => _definition.Type;
        
        [Inject]
        public void Construct(ItemDefinition definition)
        {
            _definition = definition;
            name = $"Item_{_definition.Type}";
        }

        // Zenject factory: Create(ItemDefinition) -> ItemView. The binding (in
        // GameplayInstaller) decides HOW it is produced (spawn prefab + inject).
        public class Factory : PlaceholderFactory<ItemDefinition, ItemView>
        {
        }
    }
}
