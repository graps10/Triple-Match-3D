using TripleMatch.Configs;

namespace TripleMatch.Application.Levels
{
    public interface ILevelSource
    {
        LevelDefinition GetLevel(int index);
    }
}
