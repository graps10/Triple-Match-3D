using System.Collections.Generic;
using TripleMatch.Application.Levels;
using TripleMatch.Application.Signals;
using TripleMatch.Configs;
using TripleMatch.Domain;
using TripleMatch.Presentation.Gameplay;
using UnityEngine;
using Zenject;

namespace TripleMatch.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private ItemView itemPrefab;
        [SerializeField] private List<ItemDefinition> itemDefinitions;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<ItemCollectedSignal>();
            Container.DeclareSignal<MatchMadeSignal>();
            Container.DeclareSignal<BoardBuiltSignal>();
            Container.DeclareSignal<TrayOverflowSignal>();
            Container.DeclareSignal<GameWonSignal>();
            Container.DeclareSignal<GameLostSignal>();
            Container.DeclareSignal<LevelRewardSignal>();

            // Zenject runs IInitializable.Initialize() in bind order by default (all default
            // to priority 0). GameplayOutcomeService must subscribe to BoardBuiltSignal
            // BEFORE BoardService.Initialize() fires it, so it's pinned to run first
            // explicitly instead of relying on "happens to be bound later, so it's fine".
            Container.BindExecutionOrder<GameplayOutcomeService>(-1);

            Container
                .BindFactory<ItemDefinition, ItemView, ItemView.Factory>()
                .FromComponentInNewPrefab(itemPrefab);

            // Grab the scene's Camera so InputService can raycast from it.
            Container
                .Bind<Camera>()
                .FromComponentInHierarchy()
                .AsSingle();

            // Input: a per-frame service (ITickable) that raises ItemPicked on tap.
            Container
                .BindInterfacesTo<InputService>()
                .AsSingle();

            // Both interfaces bound (not just Self) since nothing needs the concrete class -
            // ILevelLoader for BoardService, IDisposable so Zenject releases the theme on
            // scene teardown.
            Container
                .BindInterfacesTo<LevelLoader>()
                .AsSingle();

            // Board: builds the layout and reacts to picks (IInitializable/IDisposable).
            Container
                .BindInterfacesAndSelfTo<BoardService>()
                .AsSingle()
                .WithArguments(itemDefinitions);

            // Reacts to gameplay outcome signals by showing the matching popup.
            Container
                .BindInterfacesTo<GameplayFlowController>()
                .AsSingle();

            // Separate from GameplayOutcomeService (SRP) - computes stars/coins for a win
            // and reports them via LevelRewardSignal.
            Container
                .BindInterfacesTo<LevelRewardService>()
                .AsSingle();

            // Tray: 7 designer-placed slot Transforms in the Gameplay scene.
            Container
                .Bind<TraySlotsView>()
                .FromComponentInHierarchy()
                .AsSingle();

            // Match rule: pure C#, no lifecycle interfaces, so a plain interface bind is enough.
            Container
                .Bind<IMatchResolver>()
                .To<MatchResolver>()
                .AsSingle();

            // Tray: reacts to picks, groups identical items, flies them into slots.
            Container
                .BindInterfacesAndSelfTo<TrayService>()
                .AsSingle();

            // Stub SFX: subscribes to ItemCollectedSignal to prove the signal round-trip.
            Container
                .BindInterfacesTo<CollectSfxStub>()
                .AsSingle();

            // Strategy: three interchangeable IBooster implementations, all bound to the
            // same interface - Zenject collects them into one List<IBooster> for whoever
            // asks for it (see BoosterService's constructor).
            Container.Bind<IBooster>().To<UndoBooster>().AsSingle();
            Container.Bind<IBooster>().To<ShuffleBooster>().AsSingle();
            Container.Bind<IBooster>().To<ExtraSlotsBooster>().AsSingle();

            Container
                .Bind<IBoosterService>()
                .To<BoosterService>()
                .AsSingle();

            // Outcome: turns Board/Tray facts into Win/Lose and stops input via those signals.
            Container
                .BindInterfacesTo<GameplayOutcomeService>()
                .AsSingle();
        }
    }
}
