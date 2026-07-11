using System;
using TripleMatch.Application.Signals;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace TripleMatch.Presentation.Gameplay
{
    public class InputService : IInputService, ITickable, IInitializable, IDisposable
    {
        private readonly Camera _camera;
        private readonly SignalBus _signalBus;

        private bool _enabled = true;

        public event Action<ItemView> ItemPicked;

        public InputService(Camera camera, SignalBus signalBus)
        {
            _camera = camera;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<GameWonSignal>(OnGameWon);
            _signalBus.Subscribe<GameLostSignal>(OnGameLost);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<GameWonSignal>(OnGameWon);
            _signalBus.Unsubscribe<GameLostSignal>(OnGameLost);
        }

        // Either way the level is over, so stop reacting to taps.
        private void OnGameWon(GameWonSignal signal) => _enabled = false;
        private void OnGameLost(GameLostSignal signal) => _enabled = false;

        public void Tick()
        {
            if (!_enabled)
                return;

            Pointer pointer = Pointer.current;
            if (pointer == null || !pointer.press.wasPressedThisFrame)
                return;

            Vector2 screenPosition = pointer.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(screenPosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit) &&
                hit.collider.TryGetComponent(out ItemView item))
            {
                ItemPicked?.Invoke(item);
            }
        }
    }
}
