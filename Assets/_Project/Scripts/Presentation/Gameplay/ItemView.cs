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
        private Collider _collider;

        public ItemType Type => _definition.Type;

        [Inject]
        public void Construct(ItemDefinition definition)
        {
            _definition = definition;
            _collider = GetComponent<Collider>();
            name = $"Item_{_definition.Type}";
        }

        // Disabled once collected, so it can't be raycast-picked again while it
        // flies into / sits in the tray.
        public void SetInteractable(bool interactable) => _collider.enabled = interactable;

        // Zenject factory: Create(ItemDefinition) -> ItemView. The binding (in
        // GameplayInstaller) decides HOW it is produced (spawn prefab + inject).
        public class Factory : PlaceholderFactory<ItemDefinition, ItemView>
        {
        }
    }
}
