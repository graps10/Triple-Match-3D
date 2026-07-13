using System.Collections.Generic;
using TripleMatch.Configs;

namespace TripleMatch.Application.Levels
{
    public class HandcraftedLevelSource : ILevelSource
    {
        private readonly List<LevelDefinition> _levels;

        public HandcraftedLevelSource(List<LevelDefinition> levels)
        {
            _levels = levels;
        }

        public LevelDefinition GetLevel(int index) => _levels[index];
        public int Count => _levels.Count;
    }
}
