using Cysharp.Threading.Tasks;

namespace TripleMatch.Application.StateMachine
{
    public interface IState
    {
        UniTask Enter();
        UniTask Exit();
    }
}
