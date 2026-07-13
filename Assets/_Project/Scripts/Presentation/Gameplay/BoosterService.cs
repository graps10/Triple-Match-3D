using System.Collections.Generic;
using System.Linq;

namespace TripleMatch.Presentation.Gameplay
{
    public class BoosterService : IBoosterService
    {
        private readonly Dictionary<BoosterType, IBooster> _byType;

        public IReadOnlyList<IBooster> Boosters { get; }

        // Zenject collects every Container.Bind<IBooster>().To<...>() registration into
        // this single List<IBooster> automatically - no need to wire each one by hand.
        public BoosterService(List<IBooster> boosters)
        {
            Boosters = boosters;
            _byType = boosters.ToDictionary(booster => booster.Type);
        }

        public void Activate(BoosterType type) => _byType[type].Activate();
    }
}
