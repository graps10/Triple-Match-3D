using TripleMatch.Presentation.Meta;
using UnityEngine;
using Zenject;

namespace TripleMatch.Installers
{
    public class MetaInstaller : MonoInstaller
    {
        [SerializeField] private LevelButtonView levelButtonPrefab;

        public override void InstallBindings()
        {
            Container
                .BindFactory<LevelButtonView, LevelButtonView.Factory>()
                .FromComponentInNewPrefab(levelButtonPrefab);

            Container
                .Bind<LevelMapView>()
                .FromComponentInHierarchy()
                .AsSingle();

            // Spawns one button per level on Meta scene start (IInitializable) - no other
            // interface to depend on, so no need for BindInterfacesAndSelfTo here either.
            Container
                .BindInterfacesTo<LevelMapPresenter>()
                .AsSingle();
        }
    }
}
