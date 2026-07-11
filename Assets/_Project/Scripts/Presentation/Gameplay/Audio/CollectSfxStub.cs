using System;
using TripleMatch.Application.Services;
using TripleMatch.Application.Signals;
using Zenject;

namespace TripleMatch.Presentation.Gameplay
{
    public class CollectSfxStub : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly ILogService _log;

        public CollectSfxStub(SignalBus signalBus, ILogService log)
        {
            _signalBus = signalBus;
            _log = log;
        }

        public void Initialize() => _signalBus.Subscribe<ItemCollectedSignal>(OnItemCollected);

        public void Dispose() => _signalBus.Unsubscribe<ItemCollectedSignal>(OnItemCollected);

        private void OnItemCollected(ItemCollectedSignal signal) =>
            _log.Info($"SFX (stub): pop! {signal.ItemType} -> slot {signal.SlotIndex}");
    }
}
