namespace TripleMatch.Application.Services
{
    public interface IEconomyService
    {
        int Balance { get; }
        void AddCoins(int amount);
        bool TrySpend(int amount);
    }
}
