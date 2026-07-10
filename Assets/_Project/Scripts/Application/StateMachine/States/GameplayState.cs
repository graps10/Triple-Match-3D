using Cysharp.Threading.Tasks;
using TripleMatch.Application.Services;

namespace TripleMatch.Application.StateMachine.States
{
    /// <summary>
    /// In-level state. Loads the Gameplay scene when entered. Not triggered yet — it will
    /// be entered from MetaState once level selection exists (Day 13).
    /// </summary>
    public class GameplayState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ILogService _log;

        public GameplayState(ISceneLoader sceneLoader, ILogService log)
        {
            _sceneLoader = sceneLoader;
            _log = log;
        }

        public async UniTask Enter()
        {
            _log.Info("GameplayState: loading Gameplay scene...");
            await _sceneLoader.LoadAsync(GameScene.Gameplay);
        }

        public UniTask Exit() => UniTask.CompletedTask;
    }
}
