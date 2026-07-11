using TripleMatch.Configs;

namespace TripleMatch.Application.Levels
{
    public interface ILevelLoader
    {
        LevelDefinition Load();
    }
}
