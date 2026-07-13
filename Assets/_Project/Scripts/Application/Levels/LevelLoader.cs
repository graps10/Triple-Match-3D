using System;
using Cysharp.Threading.Tasks;
using TripleMatch.Application.Services;
using TripleMatch.Configs;
using TripleMatch.Domain;
using UnityEngine;

namespace TripleMatch.Application.Levels
{
    public class LevelLoader : ILevelLoader, IDisposable
    {
        private readonly ILevelSource _levelSource;
        private readonly IAssetProvider _assetProvider;
        
        private string _loadedThemeKey;

        public LevelLoader(ILevelSource levelSource, IAssetProvider assetProvider)
        {
            _levelSource = levelSource;
            _assetProvider = assetProvider;
        }

        public async UniTask<LevelDefinition> LoadAsync(int levelIndex)
        {
            LevelDefinition level = _levelSource.GetLevel(levelIndex);
            _loadedThemeKey = ToThemeKey(level.Theme);

            // The level is only "loaded" once its theme's art is ready from the bundle.
            await _assetProvider.LoadAsync<GameObject>(_loadedThemeKey);
            return level;
        }

        public void Unload()
        {
            if (_loadedThemeKey == null)
                return;

            _assetProvider.Release(_loadedThemeKey);
            _loadedThemeKey = null;
        }

        public void Dispose() => Unload();

        // GameTheme is the typed, abstraction-facing identifier; the Addressables key
        // string is an implementation detail hidden here, not leaked to callers.
        private static string ToThemeKey(GameTheme theme) => theme switch
        {
            GameTheme.Kitchen => "Theme_Kitchen",
            GameTheme.Workshop => "Theme_Workshop",
            _ => throw new ArgumentOutOfRangeException(nameof(theme), theme, null)
        };
    }
}
