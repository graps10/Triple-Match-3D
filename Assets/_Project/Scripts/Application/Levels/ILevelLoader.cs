using Cysharp.Threading.Tasks;
using TripleMatch.Configs;

namespace TripleMatch.Application.Levels
{
    public interface ILevelLoader
    {
        UniTask<LevelDefinition> LoadAsync();
        void Unload();
    }
}
