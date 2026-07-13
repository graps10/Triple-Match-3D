using System;
using UnityEngine;

namespace TripleMatch.Presentation.UI
{
    public abstract class ScreenView : MonoBehaviour
    {
        // Set by the concrete View's Construct once its Presenter exists, so every screen
        // gets its Presenter disposed on close without each View having to remember to.
        protected IDisposable Presenter { get; set; }

        protected virtual void OnDestroy() => Presenter?.Dispose();
    }
}
