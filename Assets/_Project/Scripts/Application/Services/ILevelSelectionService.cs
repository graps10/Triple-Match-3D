namespace TripleMatch.Application.Services
{
    // Project-scoped: the chosen level has to survive the Meta -> Gameplay scene
    // transition, which destroys everything scene-scoped along the way.
    public interface ILevelSelectionService
    {
        int SelectedLevelIndex { get; }
        void SelectLevel(int levelIndex);
    }
}
