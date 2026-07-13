using Cysharp.Threading.Tasks;
using TripleMatch.Application;
using TripleMatch.Application.Services;
using UnityEngine;
using Zenject;

namespace TripleMatch.Presentation.Gameplay.Debug
{
    public class LevelReloadTester : MonoBehaviour
    {
        [SerializeField] private KeyCode reloadKey = KeyCode.R;

        private ISceneLoader _sceneLoader;

        [Inject]
        public void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Update()
        {
            if (Input.GetKeyDown(reloadKey))
                _sceneLoader.LoadAsync(GameScene.Gameplay).Forget();
        }
    }
}
