using System;
using System.Collections.Generic;
using TripleMatch.Application.Services;
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
        private readonly ILogService _log;
        private readonly List<ItemDefinition> _definitions;

        private readonly List<ItemView> _items = new();

        public BoardService(
            ItemView.Factory factory,
            IInputService input,
            ILogService log,
            List<ItemDefinition> definitions)
        {
            _factory = factory;
            _input = input;
            _log = log;
            _definitions = definitions;
        }

        public void Initialize()
        {
            BuildStubBoard();
            _input.ItemPicked += OnItemPicked;
        }

        public void Dispose()
        {
            _input.ItemPicked -= OnItemPicked;
        }

        private void BuildStubBoard()
        {
            // A base row, plus one item stacked ON TOP of the first (nearer the camera),
            // so tapping that spot proves the raycast picks the top one.
            Spawn(0, new Vector3(0f, 0f, 0f));
            Spawn(1, new Vector3(1.5f, 0f, 0f));
            Spawn(2, new Vector3(3f, 0f, 0f));
            Spawn(2, new Vector3(0f, 0f, -1f)); // overlaps item #0, but closer to the camera

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
