using TripleMatch.Application;
using Zenject;

namespace TripleMatch.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameBootstrapper>().AsSingle();
        }
    }
}
