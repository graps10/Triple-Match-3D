using TripleMatch.Application.Services;
using TripleMatch.Application.StateMachine;
using TripleMatch.Infrastructure.AssetManagement;
using TripleMatch.Infrastructure.Logging;
using TripleMatch.Infrastructure.Scenes;
using Zenject;

namespace TripleMatch.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ILogService>().To<LogService>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();

            // Project-scoped: Addressables handles must survive Meta <-> Gameplay scene
            // reloads, not just a single Gameplay session (Day 11 builds on this cache).
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
        }
    }
}
