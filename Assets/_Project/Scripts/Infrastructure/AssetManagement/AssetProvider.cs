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
        private readonly Dictionary<string, int> _refCounts = new();

        public async UniTask<T> LoadAsync<T>(string key) where T : Object
        {
            if (_handles.TryGetValue(key, out AsyncOperationHandle cached))
            {
                _refCounts[key]++;
                return cached.Convert<T>().Result;
            }

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            await handle.ToUniTask();

            _handles[key] = handle;
            _refCounts[key] = 1;
            return handle.Result;
        }

        public void Release(string key)
        {
            if (!_handles.TryGetValue(key, out AsyncOperationHandle handle))
                return;

            _refCounts[key]--;
            if (_refCounts[key] > 0)
                return;

            Addressables.Release(handle);
            _handles.Remove(key);
            _refCounts.Remove(key);
        }
    }
}
