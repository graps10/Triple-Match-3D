using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TripleMatch.Application.Services
{
    public interface IAssetProvider
    {
        UniTask<T> LoadAsync<T>(string key) where T : Object;

        // Decrements the key's reference count; only releases the underlying Addressables
        // handle back to Unity once the count reaches zero (last borrower wins).
        void Release(string key);
    }
}
