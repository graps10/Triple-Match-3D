using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TripleMatch.Application.Levels;
using TripleMatch.Application.Services;
using TripleMatch.Application.Signals;
using TripleMatch.Configs;
using TripleMatch.Domain;
using UnityEngine;
using Zenject;

namespace TripleMatch.Presentation.Gameplay
{
    public class BoardService : IBoardService, IInitializable, IDisposable
    {
        private const float Layer_Depth_Step = 0.5f;
        private const float Shuffle_Duration = 0.4f;
        private const float Shuffle_Jump_Power = 1f;

        private readonly ItemView.Factory _factory;
        private readonly IInputService _input;
        private readonly SignalBus _signalBus;
        private readonly ILogService _log;
        private readonly ILevelLoader _levelLoader;
        private readonly Dictionary<ItemType, ItemDefinition> _definitionsByType;
        private readonly ILevelSelectionService _levelSelection;
        private readonly BoardOccupancy _occupancy = new();

        private readonly List<ItemView> _items = new();

        // Fired only for picks BoardService actually allows through (see OnItemPicked).
        // TrayService listens to this instead of the raw IInputService event.
        public event Action<ItemView> ItemPickValidated;

        public BoardService(
            ItemView.Factory factory,
            IInputService input,
            SignalBus signalBus,
            ILogService log,
            ILevelLoader levelLoader,
            List<ItemDefinition> definitions,
            ILevelSelectionService levelSelection)
        {
            _factory = factory;
            _input = input;
            _signalBus = signalBus;
            _log = log;
            _levelLoader = levelLoader;
            _definitionsByType = definitions.ToDictionary(definition => definition.Type);
            _levelSelection = levelSelection;
        }

        public void Initialize()
        {
            // Initialize() can't be async itself (Zenject's IInitializable contract is
            // synchronous), so kick off the load as fire-and-forget UniTaskVoid instead.
            BuildBoardAsync().Forget();
            _input.ItemPicked += OnItemPicked;
        }

        public void Dispose()
        {
            _input.ItemPicked -= OnItemPicked;
        }

        private async UniTaskVoid BuildBoardAsync()
        {
            LevelDefinition level = await _levelLoader.LoadAsync(_levelSelection.SelectedLevelIndex);

            foreach (LevelItemEntry entry in level.Items)
                Spawn(entry);

            _log.Info($"Board built with {_items.Count} items. Tap them!");
            _signalBus.Fire(new BoardBuiltSignal(_items.Count));
        }

        private void Spawn(LevelItemEntry entry)
        {
            ItemDefinition definition = _definitionsByType[entry.Type];
            ItemView item = _factory.Create(definition);

            // Higher layer = closer to the camera = picked first when items overlap.
            float z = entry.Position.y - entry.Layer * Layer_Depth_Step;
            item.transform.position = new Vector3(entry.Position.x, 0f, z);
            item.PlaceOnBoard(entry.Position, entry.Layer);

            _occupancy.Register(entry.Position.x, entry.Position.y, entry.Layer);
            _items.Add(item);
        }

        public void ReturnToBoard(ItemView item, Vector2 position, int layer)
        {
            item.PlaceOnBoard(position, layer);

            float z = position.y - layer * Layer_Depth_Step;
            item.transform.position = new Vector3(position.x, 0f, z);
            item.SetInteractable(true);

            _occupancy.Register(position.x, position.y, layer);
            _items.Add(item);
        }

        public void Shuffle()
        {
            // Reuse the exact set of positions/layers already on the board - no new
            // positions invented, so this can never collide with anything.
            List<(Vector2 Position, int Layer)> slots = _items
                .Select(item => (item.Position, item.Layer))
                .ToList();
            ShuffleInPlace(slots);

            _occupancy.Clear();
            for (int i = 0; i < _items.Count; i++)
            {
                ItemView item = _items[i];
                (Vector2 position, int layer) = slots[i];

                // Logical position updates immediately (occlusion/picking must be correct
                // right away); the transform only catches up visually, via the jump below -
                // same "logic first, animation follows" split TrayService already uses.
                item.PlaceOnBoard(position, layer);
                _occupancy.Register(position.x, position.y, layer);

                float z = position.y - layer * Layer_Depth_Step;
                Vector3 target = new Vector3(position.x, 0f, z);

                item.transform.DOKill();
                item.transform.DOJump(target, Shuffle_Jump_Power, 1, Shuffle_Duration).SetEase(Ease.OutQuad);
            }
        }

        private static void ShuffleInPlace<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        private void OnItemPicked(ItemView item)
        {
            // Raw raycast hits can still land on a covered item (placeholder cubes /
            // camera angle don't always occlude perfectly) - reject anything that isn't
            // topmost at its position instead of trusting physics alone.
            if (!_occupancy.IsTopmost(item.Position.x, item.Position.y, item.Layer))
            {
                _log.Info($"Ignored pick on covered item: {item.Type} (layer {item.Layer})");
                return;
            }

            _occupancy.Remove(item.Position.x, item.Position.y, item.Layer);

            // Item is leaving the board for the tray (TrayService handles the fly-in);
            // the board just stops tracking it.
            _items.Remove(item);
            _log.Info($"Picked: {item.Type} (at {item.transform.position})");
            ItemPickValidated?.Invoke(item);
        }
    }
}
