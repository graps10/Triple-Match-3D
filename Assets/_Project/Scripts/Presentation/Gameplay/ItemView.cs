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
        public Vector2 Position { get; private set; }
        public int Layer { get; private set; }

        [Inject]
        public void Construct(ItemDefinition definition)
        {
            _definition = definition;
            _collider = GetComponent<Collider>();
            name = $"Item_{_definition.Type}";
        }

        // Board placement (as opposed to Construct's DI-provided data) - set once by
        // BoardService right after spawning, so InputService/BoardService can later ask
        // "is this the topmost item at its position" without recomputing world coordinates.
        public void PlaceOnBoard(Vector2 position, int layer)
        {
            Position = position;
            Layer = layer;
        }

        // Disabled once collected, so it can't be raycast-picked again while it
        // flies into / sits in the tray.
        public void SetInteractable(bool interactable) => _collider.enabled = interactable;

        // Zenject factory: Create(ItemDefinition) -> ItemView. The binding (in
        // GameplayInstaller) decides HOW it is produced (spawn prefab + inject).
        public class Factory : PlaceholderFactory<ItemDefinition, ItemView> { }
    }
}
