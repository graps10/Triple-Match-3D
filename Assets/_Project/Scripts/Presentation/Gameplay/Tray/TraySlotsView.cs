using TripleMatch.Domain;
using UnityEngine;

namespace TripleMatch.Presentation.Gameplay
{
    public class TraySlotsView : MonoBehaviour
    {
        [SerializeField] private Transform[] slots = new Transform[Tray.CAPACITY];

        public Transform this[int index] => slots[index];
    }
}
