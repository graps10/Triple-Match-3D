using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TripleMatch.Presentation.UI
{
    public class ScreenService : IScreenService
    {
        private readonly DiContainer _container;
        private readonly UIRoot _uiRoot;
        private readonly Dictionary<ScreenId, ScreenView> _prefabsById;
        private readonly Stack<ScreenView> _stack = new();

        public ScreenService(DiContainer container, UIRoot uiRoot, List<ScreenBinding> bindings)
        {
            _container = container;
            _uiRoot = uiRoot;
            _prefabsById = bindings.ToDictionary(binding => binding.Id, binding => binding.Prefab);
        }

        public TView Push<TView>(ScreenId id, params object[] extraArgs) where TView : ScreenView
        {
            ScreenView prefab = _prefabsById[id];

            // Screens under it stay visible (not hidden) - new siblings render on top by
            // default Canvas ordering, so this alone gives the "popup over HUD" look.
            TView view = _container.InstantiatePrefabForComponent<TView>(prefab, _uiRoot.ScreenParent, extraArgs);
            _stack.Push(view);
            return view;
        }

        public void Pop()
        {
            if (_stack.Count == 0)
                return;

            ScreenView top = _stack.Pop();
            Object.Destroy(top.gameObject);
        }
    }
}
