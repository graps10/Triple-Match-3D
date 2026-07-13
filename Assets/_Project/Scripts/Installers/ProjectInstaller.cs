using System.Collections.Generic;
using TripleMatch.Application.Levels;
using TripleMatch.Application.Services;
using TripleMatch.Application.StateMachine;
using TripleMatch.Configs;
using TripleMatch.Infrastructure.AssetManagement;
using TripleMatch.Infrastructure.Logging;
using TripleMatch.Infrastructure.Scenes;
using TripleMatch.Presentation.UI;
using UnityEngine;
using Zenject;

namespace TripleMatch.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private List<LevelDefinition> levels;
        [SerializeField] private UIRoot uiRootPrefab;
        [SerializeField] private List<ScreenBinding> screens;

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

            // Project-scoped: coins earned on a level must still show after returning to Meta.
            Container.Bind<IEconomyService>().To<EconomyService>().AsSingle();

            // UnderTransform(transform) parents the spawned UIRoot under ProjectContext
            // itself, so it inherits ProjectContext's DontDestroyOnLoad instead of being
            // destroyed on the very next scene load like anything scene-scoped would be.
            Container
                .Bind<UIRoot>()
                .FromComponentInNewPrefab(uiRootPrefab)
                .UnderTransform(transform)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<IScreenService>()
                .To<ScreenService>()
                .AsSingle()
                .WithArguments(screens)
                .NonLazy();

            // Transient: a fresh Presenter per screen instance, matching the fresh View
            // ScreenService.Push spawns each time (not one Presenter shared for the
            // lifetime of the whole game).
            Container.Bind<HudPresenter>().AsTransient();
            Container.Bind<PopupPresenter>().AsTransient();
            Container.Bind<WinPopupPresenter>().AsTransient();
            Container.Bind<LosePopupPresenter>().AsTransient();
        }
    }
}
