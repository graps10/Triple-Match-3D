using System;

namespace TripleMatch.Presentation.Gameplay
{
    public interface IInputService
    {
        event Action<ItemView> ItemPicked;
    }
}
