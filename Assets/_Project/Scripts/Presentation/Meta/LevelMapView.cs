using UnityEngine;

namespace TripleMatch.Presentation.Meta
{
    // Scene marker (mirrors TraySlotsView's role in Gameplay) - points at the Transform
    // spawned level buttons get parented under.
    public class LevelMapView : MonoBehaviour
    {
        [SerializeField] private Transform buttonsContainer;

        public Transform ButtonsContainer => buttonsContainer;
    }
}
