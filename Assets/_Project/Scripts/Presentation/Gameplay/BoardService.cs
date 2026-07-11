using System;
using System.Collections.Generic;
using TripleMatch.Application.Services;
using TripleMatch.Application.Signals;
using TripleMatch.Configs;
using UnityEngine;
using Zenject;

namespace TripleMatch.Presentation.Gameplay
{
    /// <summary>
    /// Builds and owns the board of items. For now it spawns a small stub layout in layers
    /// (one item sits closer to the camera = "on top" of another) and logs which item the
    /// player picks. Real level data + tray routing arrive in later days.
    /// </summary>
    public class BoardService : IInitializable, IDisposable
    {
        private readonly ItemView.Factory _factory;
        private readonly IInputService _input;
        private readonly SignalBus _signalBus;
        private readonly ILogService _log;
        private readonly List<ItemDefinition> _definitions;

        private readonly List<ItemView> _items = new();

        public BoardService(
            ItemView.Factory factory,
            IInputService input,
            SignalBus signalBus,
            ILogService log,
            List<ItemDefinition> definitions)
        {
            _factory = factory;
            _input = input;
            _signalBus = signalBus;
            _log = log;
            _definitions = definitions;
        }

        public void Initialize()
        {
            BuildStubBoard();
            _signalBus.Fire(new BoardBuiltSignal(_items.Count));
            _input.ItemPicked += OnItemPicked;
        }

        public void Dispose()
        {
            _input.ItemPicked -= OnItemPicked;
        }

        private void BuildStubBoard()
        {
            // Two matching trios: the whole board is clearable, so tapping everything
            // proves the Win path (remaining count reaches 0). Day 9 replaces this stub
            // with real LevelDefinition data.
            Spawn(0, new Vector3(-1.5f, 0f, 0f));
            Spawn(0, new Vector3(-0.5f, 0f, 0f));
            Spawn(0, new Vector3(0.5f, 0f, 0f));
            Spawn(1, new Vector3(1.5f, 0f, 0f));
            Spawn(1, new Vector3(2.5f, 0f, 0f));
            Spawn(1, new Vector3(3.5f, 0f, 0f));
            Spawn(1, new Vector3(3.5f, 0f, 0f));

            _log.Info($"Board built with {_items.Count} items. Tap them!");
        }

        private void Spawn(int definitionIndex, Vector3 position)
        {
            ItemDefinition definition = _definitions[definitionIndex % _definitions.Count];
            ItemView item = _factory.Create(definition);
            item.transform.position = position;
            _items.Add(item);
        }

        private void OnItemPicked(ItemView item)
        {
            // Item is leaving the board for the tray (TrayService handles the fly-in);
            // the board just stops tracking it.
            _items.Remove(item);
            _log.Info($"Picked: {item.Type} (at {item.transform.position})");
        }
    }
}
