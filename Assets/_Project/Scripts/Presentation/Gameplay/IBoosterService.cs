using System.Collections.Generic;

namespace TripleMatch.Presentation.Gameplay
{
    public interface IBoosterService
    {
        IReadOnlyList<IBooster> Boosters { get; }
        void Activate(BoosterType type);
    }
}
