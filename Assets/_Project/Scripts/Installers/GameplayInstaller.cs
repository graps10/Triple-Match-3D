using System.Collections.Generic;
using TripleMatch.Application.Signals;
using TripleMatch.Configs;
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

            // Board: builds the layout and reacts to picks (IInitializable/IDisposable).
            Container
                .BindInterfacesAndSelfTo<BoardService>()
                .AsSingle()
                .WithArguments(itemDefinitions);

            // Tray: 7 designer-placed slot Transforms in the Gameplay scene.
            Container
                .Bind<TraySlotsView>()
                .FromComponentInHierarchy()
                .AsSingle();

            // Tray: reacts to picks, groups identical items, flies them into slots.
            Container
                .BindInterfacesAndSelfTo<TrayService>()
                .AsSingle();

            // Stub SFX: subscribes to ItemCollectedSignal to prove the signal round-trip.
            Container
                .BindInterfacesTo<CollectSfxStub>()
                .AsSingle();
        }
    }
}
