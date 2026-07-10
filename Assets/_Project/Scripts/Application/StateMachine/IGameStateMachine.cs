using Cysharp.Threading.Tasks;

namespace TripleMatch.Application.StateMachine
{
    public interface IGameStateMachine
    {
        UniTask Enter<TState>() where TState : IState;
    }
}
