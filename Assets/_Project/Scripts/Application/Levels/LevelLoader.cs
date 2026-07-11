using TripleMatch.Configs;

namespace TripleMatch.Application.Levels
{
    public class LevelLoader : ILevelLoader
    {
        private const int Default_Level_Index = 0;

        private readonly ILevelSource _levelSource;

        public LevelLoader(ILevelSource levelSource)
        {
            _levelSource = levelSource;
        }

        public LevelDefinition Load() => _levelSource.GetLevel(Default_Level_Index);
    }
}
