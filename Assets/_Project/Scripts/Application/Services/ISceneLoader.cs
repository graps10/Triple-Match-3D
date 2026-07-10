using Cysharp.Threading.Tasks;

namespace TripleMatch.Application.Services
{
    /// <summary>
    /// Abstraction over scene loading. Takes a type-safe <see cref="GameScene"/> so callers
    /// can never pass an invalid scene. The concrete implementation maps it to a real scene.
    /// </summary>
    public interface ISceneLoader
    {
        UniTask LoadAsync(GameScene scene);
    }
}
