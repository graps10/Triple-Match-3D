using System;
using Cysharp.Threading.Tasks;
using TripleMatch.Application.Services;
using TripleMatch.Configs;
using UnityEngine;

namespace TripleMatch.Application.Levels
{
    public class LevelLoader : ILevelLoader, IDisposable
    {
        private const int Default_Level_Index = 0;
        private const string Theme_Key = "Theme_Kitchen";

        private readonly ILevelSource _levelSource;
        private readonly IAssetProvider _assetProvider;

        public LevelLoader(ILevelSource levelSource, IAssetProvider assetProvider)
        {
            _levelSource = levelSource;
            _assetProvider = assetProvider;
        }

        public async UniTask<LevelDefinition> LoadAsync()
        {
            // The level is only "loaded" once its theme's art is ready from the bundle.
            await _assetProvider.LoadAsync<GameObject>(Theme_Key);
            return _levelSource.GetLevel(Default_Level_Index);
        }

        public void Unload() => _assetProvider.Release(Theme_Key);
        public void Dispose() => Unload();
    }
}
