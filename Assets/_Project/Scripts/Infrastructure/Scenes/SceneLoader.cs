using Cysharp.Threading.Tasks;
using TripleMatch.Application;
using TripleMatch.Application.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TripleMatch.Infrastructure.Scenes
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask LoadAsync(GameScene scene)
        {
            await SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Single);
        }
    }
}
