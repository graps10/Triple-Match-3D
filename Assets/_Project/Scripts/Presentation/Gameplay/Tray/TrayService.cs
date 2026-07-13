using System;
using System.Collections.Generic;
using DG.Tweening;
using TripleMatch.Application.Services;
using TripleMatch.Application.Signals;
using TripleMatch.Domain;
using UnityEngine;
using Zenject;

namespace TripleMatch.Presentation.Gameplay
{
    public class TrayService : ITrayService, IInitializable, IDisposable
    {
        private const float Flight_Duration = 0.35f;

        private readonly IBoardService _board;
        private readonly IMatchResolver _matchResolver;
        private readonly SignalBus _signalBus;
        private readonly ILogService _log;
        private readonly TraySlotsView _slotsView;

        private readonly Tray _tray = new();
        private readonly List<ItemView> _slotItems = new();

        // Only the most recent Collect - overwritten by the next one, cleared the moment
        // a match consumes it (nothing left to undo at that point).
        private UndoCollectCommand _lastCollect;

        public bool IsFull => _tray.IsFull;
        public bool CanUndo => _lastCollect != null;

        public TrayService(
            IBoardService board,
            IMatchResolver matchResolver,
            SignalBus signalBus,
            ILogService log,
            TraySlotsView slotsView)
        {
            _board = board;
            _matchResolver = matchResolver;
            _signalBus = signalBus;
            _log = log;
            _slotsView = slotsView;
        }

        public void Initialize() => _board.ItemPickValidated += OnItemPicked;

        public void Dispose() => _board.ItemPickValidated -= OnItemPicked;

        private void OnItemPicked(ItemView item) => Collect(item);

        public void Collect(ItemView item)
        {
            if (_tray.IsFull)
            {
                _log.Info("Tray is full, cannot collect item.");
                return;
            }

            Vector2 boardPosition = item.Position;
            int boardLayer = item.Layer;

            item.SetInteractable(false);

            int slotIndex = _tray.Collect(item.Type);
            _slotItems.Insert(slotIndex, item);
            _lastCollect = new UndoCollectCommand(item, slotIndex, boardPosition, boardLayer);

            FlyItemsToSlots();

            _signalBus.Fire(new ItemCollectedSignal(item.Type, slotIndex));

            bool matched = TryResolveMatch();
            if (matched)
                _lastCollect = null;

            // Lose condition: the tray filled up and this collect didn't free any slots.
            if (!matched && _tray.IsFull)
                _signalBus.Fire(new TrayOverflowSignal());
        }

        public void Undo()
        {
            if (_lastCollect == null)
                return;

            _tray.RemoveAt(_lastCollect.SlotIndex);
            _slotItems.RemoveAt(_lastCollect.SlotIndex);

            _board.ReturnToBoard(_lastCollect.Item, _lastCollect.BoardPosition, _lastCollect.BoardLayer);

            FlyItemsToSlots();
            _lastCollect = null;
        }

        private bool TryResolveMatch()
        {
            MatchResult result = _matchResolver.Resolve(_tray.Slots);
            if (!result.Matched)
                return false;

            _tray.RemoveRun(result.SlotIndex, result.Length);
            RemoveMatchedViews(result.SlotIndex, result.Length);

            FlyItemsToSlots();

            _signalBus.Fire(new MatchMadeSignal(result.Type, result.SlotIndex, result.Length));
            return true;
        }

        private void RemoveMatchedViews(int startIndex, int length)
        {
            for (int i = 0; i < length; i++)
            {
                ItemView view = _slotItems[startIndex];
                _slotItems.RemoveAt(startIndex);

                // Kill any in-flight DOTween tween first, or it tries to update this
                // Transform next frame after Destroy and throws a missing-target error.
                view.transform.DOKill();
                UnityEngine.Object.Destroy(view.gameObject);
            }
        }

        private void FlyItemsToSlots()
        {
            for (int i = 0; i < _slotItems.Count; i++)
            {
                _slotItems[i].transform
                    .DOMove(_slotsView[i].position, Flight_Duration)
                    .SetEase(Ease.OutBack);
            }
        }
    }
}
