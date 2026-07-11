using System;
using System.Collections.Generic;
using DG.Tweening;
using TripleMatch.Application.Services;
using TripleMatch.Application.Signals;
using TripleMatch.Domain;
using Zenject;

namespace TripleMatch.Presentation.Gameplay
{
    public class TrayService : ITrayService, IInitializable, IDisposable
    {
        private const float Flight_Duration = 0.35f;

        private readonly IInputService _input;
        private readonly IMatchResolver _matchResolver;
        private readonly SignalBus _signalBus;
        private readonly ILogService _log;
        private readonly TraySlotsView _slotsView;

        private readonly Tray _tray = new();
        private readonly List<ItemView> _slotItems = new();

        public bool IsFull => _tray.IsFull;

        public TrayService(
            IInputService input,
            IMatchResolver matchResolver,
            SignalBus signalBus,
            ILogService log,
            TraySlotsView slotsView)
        {
            _input = input;
            _matchResolver = matchResolver;
            _signalBus = signalBus;
            _log = log;
            _slotsView = slotsView;
        }

        public void Initialize() => _input.ItemPicked += OnItemPicked;

        public void Dispose() => _input.ItemPicked -= OnItemPicked;

        private void OnItemPicked(ItemView item) => Collect(item);

        public void Collect(ItemView item)
        {
            if (_tray.IsFull)
            {
                _log.Info("Tray is full, cannot collect item.");
                return;
            }

            item.SetInteractable(false);

            int slotIndex = _tray.Collect(item.Type);
            _slotItems.Insert(slotIndex, item);

            FlyItemsToSlots();

            _signalBus.Fire(new ItemCollectedSignal(item.Type, slotIndex));

            TryResolveMatch();
        }

        private void TryResolveMatch()
        {
            MatchResult result = _matchResolver.Resolve(_tray.Slots);
            if (!result.Matched)
                return;

            _tray.RemoveRun(result.SlotIndex, result.Length);
            RemoveMatchedViews(result.SlotIndex, result.Length);

            FlyItemsToSlots();

            _signalBus.Fire(new MatchMadeSignal(result.Type, result.SlotIndex));
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
