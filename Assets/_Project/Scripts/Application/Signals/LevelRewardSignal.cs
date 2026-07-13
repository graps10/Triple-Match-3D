namespace TripleMatch.Application.Signals
{
    public class LevelRewardSignal
    {
        public int Stars { get; }
        public int Coins { get; }

        public LevelRewardSignal(int stars, int coins)
        {
            Stars = stars;
            Coins = coins;
        }
    }
}
