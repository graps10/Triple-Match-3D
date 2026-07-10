using Cysharp.Threading.Tasks;
using TripleMatch.Application.Services;

namespace TripleMatch.Application.StateMachine.States
{
    /// <summary>
    /// Main menu / level map. For now it just loads the Meta scene. Later it will host
    /// level selection and transition into GameplayState with the chosen level.
    /// </summary>
    public class MetaState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ILogService _log;

        public MetaState(ISceneLoader sceneLoader, ILogService log)
        {
            _sceneLoader = sceneLoader;
            _log = log;
        }

        public async UniTask Enter()
        {
            _log.Info("MetaState: loading Meta scene...");
            await _sceneLoader.LoadAsync(GameScene.Meta);
            _log.Info("MetaState: Meta scene is ready.");
        }

        public UniTask Exit() => UniTask.CompletedTask;
    }
}
