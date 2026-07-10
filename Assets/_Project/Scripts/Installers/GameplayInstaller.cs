using TripleMatch.Configs;
using TripleMatch.Presentation.Gameplay;
using UnityEngine;
using Zenject;

namespace TripleMatch.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private ItemView itemPrefab;
        [SerializeField] private ItemDefinition testDefinition;

        public override void InstallBindings()
        {
            Container
                .BindFactory<ItemDefinition, ItemView, ItemView.Factory>()
                .FromComponentInNewPrefab(itemPrefab);

            // TEMPORARY: spawns a few items on scene start to prove the factory works.
            Container
                .BindInterfacesTo<ItemSpawnerTest>()
                .AsSingle()
                .WithArguments(testDefinition);
        }
    }
}
