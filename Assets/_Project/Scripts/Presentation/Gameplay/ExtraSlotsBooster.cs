namespace TripleMatch.Presentation.Gameplay
{
    // Stub for today (interface satisfied, no behavior) - real design (per the user):
    // moves up to the last 3 tray items into a separate, layer-free holding area (fewer
    // if the tray has fewer than 3 items right now); tapping an item there sends it back
    // into the main tray as if freshly collected. A bigger feature for a later day.
    public class ExtraSlotsBooster : IBooster
    {
        private const int Booster_Cost = 25;

        public BoosterType Type => BoosterType.ExtraSlots;
        public int Cost => Booster_Cost;

        public bool CanActivate() => false;

        public void Activate()
        {
        }
    }
}
