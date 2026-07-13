using Cysharp.Threading.Tasks;
using TripleMatch.Application.Services;

namespace TripleMatch.Application.StateMachine.States
{
    /// <summary>
    /// Main menu / level map. Loads the Meta scene; level selection itself lives in
    /// Meta's own LevelMapPresenter (scene-scoped), not here.
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
