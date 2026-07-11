namespace TripleMatch.Application.Signals
{
    // Fired by TrayService the moment the tray is full (7/7) and no match was
    // possible on the last collect — the lose condition.
    public class TrayOverflowSignal
    {
    }
}
