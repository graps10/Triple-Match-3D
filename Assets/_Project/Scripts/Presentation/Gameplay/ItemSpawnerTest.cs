using TripleMatch.Application.Services;
using TripleMatch.Configs;
using UnityEngine;
using Zenject;

namespace TripleMatch.Presentation.Gameplay
{
    /// <summary>
    /// TEMPORARY smoke test for the item factory: spawns a small row of items when the
    /// Gameplay scene starts, proving the factory + injection works. Will be replaced by
    /// BoardService (Day 5).
    /// </summary>
    public class ItemSpawnerTest : IInitializable
    {
        private readonly ItemView.Factory _factory;
        private readonly ItemDefinition _definition;
        private readonly ILogService _log;

        public ItemSpawnerTest(ItemView.Factory factory, ItemDefinition definition, ILogService log)
        {
            _factory = factory;
            _definition = definition;
            _log = log;
        }

        public void Initialize()
        {
            for (int i = 0; i < 3; i++)
            {
                ItemView item = _factory.Create(_definition);
                item.transform.position = new Vector3(i * 1.5f, 0f, 0f);
            }

            _log.Info($"Spawned 3 items via factory (type: {_definition.Type}).");
        }
    }
}
