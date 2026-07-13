using TripleMatch.Application.Services;

namespace TripleMatch.Presentation.Gameplay
{
    public class ShuffleBooster : IBooster
    {
        private const int Booster_Cost = 15;

        private readonly IBoardService _board;
        private readonly IEconomyService _economy;

        public BoosterType Type => BoosterType.Shuffle;
        public int Cost => Booster_Cost;

        public ShuffleBooster(IBoardService board, IEconomyService economy)
        {
            _board = board;
            _economy = economy;
        }

        public bool CanActivate() => _economy.Balance >= Cost;

        public void Activate()
        {
            if (!CanActivate())
                return;

            _economy.TrySpend(Cost);
            _board.Shuffle();
        }
    }
}
