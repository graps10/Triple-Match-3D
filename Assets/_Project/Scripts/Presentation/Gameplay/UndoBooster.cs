using TripleMatch.Application.Services;

namespace TripleMatch.Presentation.Gameplay
{
    public class UndoBooster : IBooster
    {
        private const int Booster_Cost = 20;

        private readonly ITrayService _tray;
        private readonly IEconomyService _economy;

        public BoosterType Type => BoosterType.Undo;
        public int Cost => Booster_Cost;

        public UndoBooster(ITrayService tray, IEconomyService economy)
        {
            _tray = tray;
            _economy = economy;
        }

        public bool CanActivate() => _tray.CanUndo && _economy.Balance >= Cost;

        public void Activate()
        {
            if (!CanActivate())
                return;

            _economy.TrySpend(Cost);
            _tray.Undo();
        }
    }
}
