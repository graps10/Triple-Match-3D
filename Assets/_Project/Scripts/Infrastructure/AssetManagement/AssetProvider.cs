using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TripleMatch.Application.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TripleMatch.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _handles = new();

        public async UniTask<T> LoadAsync<T>(string key) where T : Object
        {
            if (_handles.TryGetValue(key, out AsyncOperationHandle cached))
                return cached.Convert<T>().Result;

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            await handle.ToUniTask();

            _handles[key] = handle;
            return handle.Result;
        }
    }
}
