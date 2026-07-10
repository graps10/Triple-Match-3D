using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace TripleMatch.Presentation.Gameplay
{
    public class InputService : IInputService, ITickable
    {
        private readonly Camera _camera;

        public event Action<ItemView> ItemPicked;

        public InputService(Camera camera)
        {
            _camera = camera;
        }

        public void Tick()
        {
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
