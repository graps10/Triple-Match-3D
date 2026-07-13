using System.Collections.Generic;
using TripleMatch.Application.Levels;
using TripleMatch.Application.Services;
using TripleMatch.Application.StateMachine;
using TripleMatch.Configs;
using TripleMatch.Infrastructure.AssetManagement;
using TripleMatch.Infrastructure.Logging;
using TripleMatch.Infrastructure.Scenes;
using UnityEngine;
using Zenject;

namespace TripleMatch.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private List<LevelDefinition> levels;

        public override void InstallBindings()
        {
            Container.Bind<ILogService>().To<LogService>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();

            // Project-scoped: Addressables handles must survive Meta <-> Gameplay scene
            // reloads, not just a single Gameplay session (Day 11 builds on this cache).
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();

            // Project-scoped: both Meta (level map needs the count) and Gameplay (needs
            // one specific level) must read the same level list across scene reloads.
            Container
                .Bind<ILevelSource>()
                .To<HandcraftedLevelSource>()
                .AsSingle()
                .WithArguments(levels);

            // Project-scoped: written by Meta's level map, read by Gameplay's BoardService
            // after the scene transition - a plain SceneContext field wouldn't survive it.
            Container.Bind<ILevelSelectionService>().To<LevelSelectionService>().AsSingle();
        }
    }
}
