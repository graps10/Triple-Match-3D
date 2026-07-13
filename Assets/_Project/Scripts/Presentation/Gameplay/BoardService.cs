using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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
