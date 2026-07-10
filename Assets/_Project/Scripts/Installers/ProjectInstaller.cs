using TripleMatch.Application;
using TripleMatch.Application.Services;
using TripleMatch.Infrastructure.Logging;
using Zenject;

namespace TripleMatch.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ILogService>()
                .To<LogService>()
                .AsSingle();
            
            Container
                .BindInterfacesTo<GreeterService>()
                .AsSingle();
        }
    }
}
