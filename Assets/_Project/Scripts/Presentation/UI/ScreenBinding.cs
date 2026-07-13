using System;
using UnityEngine;

namespace TripleMatch.Presentation.UI
{
    [Serializable]
    public struct ScreenBinding
    {
        [SerializeField] private ScreenId id;
        [SerializeField] private ScreenView prefab;

        public ScreenId Id => id;
        public ScreenView Prefab => prefab;
    }
}
