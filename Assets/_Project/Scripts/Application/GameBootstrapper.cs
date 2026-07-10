using Cysharp.Threading.Tasks;
using TripleMatch.Application.StateMachine;
using TripleMatch.Application.StateMachine.States;
using Zenject;

namespace TripleMatch.Application
{
    public class GameBootstrapper : IInitializable
    {
        private readonly IGameStateMachine _machine;

        public GameBootstrapper(IGameStateMachine machine)
        {
            _machine = machine;
        }

        public void Initialize()
        {
            // Initialize() is synchronous (void). We start the async flow and intentionally
            // do not await it here — .Forget() tells UniTask that's on purpose (and still
            // surfaces exceptions to the console).
            _machine.Enter<BootState>().Forget();
        }
    }
}
