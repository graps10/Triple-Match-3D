using TripleMatch.Application.Services;
using Zenject;

namespace TripleMatch.Application
{
    public class GreeterService : IInitializable
    {
        private readonly ILogService _log;
        
        public GreeterService(ILogService log) => _log = log;

        public void Initialize()
        {
            _log.Info("Hello DI! GreeterService was built by Zenject and got ILogService via its constructor — no new(), no FindObjectOfType, no singleton.");
        }
    }
}
