namespace TripleMatch.Application.Services
{
    public class EconomyService : IEconomyService
    {
        public int Balance { get; private set; }

        public void AddCoins(int amount) => Balance += amount;
    }
}
