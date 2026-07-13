namespace TripleMatch.Application.Services
{
    public class LevelSelectionService : ILevelSelectionService
    {
        public int SelectedLevelIndex { get; private set; }

        public void SelectLevel(int levelIndex) => SelectedLevelIndex = levelIndex;
    }
}
