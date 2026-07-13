namespace TripleMatch.Application.Services
{
    public class EconomyService : IEconomyService
    {
        public int Balance { get; private set; }

        public void AddCoins(int amount) => Balance += amount;

        public bool TrySpend(int amount)
        {
            if (Balance < amount)
                return false;

            Balance -= amount;
            return true;
        }
    }
}
