using UnityEngine;
using Zenject;

namespace TripleMatch.Presentation.UI.Debug
{
    // Temporary: nothing pushes real screens yet (Days 15/18 add real HUD/popup content
    // and their own trigger points) - this proves Push/Pop work end-to-end in Play mode.
    // Delete once a real caller (e.g. HUD-on-gameplay-start) exists.
    public class ScreenStackTester : MonoBehaviour
    {
        [SerializeField] private KeyCode pushHudKey = KeyCode.H;
        [SerializeField] private KeyCode pushPopupKey = KeyCode.P;

        private IScreenService _screenService;

        [Inject]
        public void Construct(IScreenService screenService)
        {
            _screenService = screenService;
        }

        private void Update()
        {
            if (Input.GetKeyDown(pushHudKey))
                _screenService.Push<HudView>(ScreenId.Hud);

            if (Input.GetKeyDown(pushPopupKey))
                _screenService.Push<PopupView>(ScreenId.Popup);
        }
    }
}
