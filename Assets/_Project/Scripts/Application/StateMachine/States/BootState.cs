using Cysharp.Threading.Tasks;
using TripleMatch.Application.Services;

namespace TripleMatch.Application.StateMachine.States
{
    /// <summary>
    /// First state of the game. The place to warm up core systems (load save progress,
    /// prime Addressables, etc.). For now it just auto-advances to the Meta state.
    /// </summary>
    public class BootState : IState
    {
        private readonly IGameStateMachine _machine;
        private readonly ILogService _log;

        public BootState(IGameStateMachine machine, ILogService log)
        {
            _machine = machine;
            _log = log;
        }

        public async UniTask Enter()
        {
            _log.Info("BootState: warming up core systems...");
            // (later: load progress, warm Addressables, splash timing, etc.)
            await _machine.Enter<MetaState>();
        }

        public UniTask Exit()
        {
            _log.Info("BootState: done, handing off to Meta.");
            return UniTask.CompletedTask;
        }
    }
}
