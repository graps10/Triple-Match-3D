using UnityEngine;

namespace TripleMatch.Presentation.UI
{
    // Spawned once as a child of ProjectContext (see ProjectInstaller) so it survives
    // every scene load - screens pushed here stay on screen across Meta <-> Gameplay.
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private Transform screenParent;

        public Transform ScreenParent => screenParent;
    }
}
