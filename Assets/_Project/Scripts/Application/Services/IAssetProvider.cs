using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TripleMatch.Application.Services
{
    public interface IAssetProvider
    {
        UniTask<T> LoadAsync<T>(string key) where T : Object;
    }
}
