using TripleMatch.Application.Services;
using UnityEngine;

namespace TripleMatch.Infrastructure.Logging
{
    public class LogService : ILogService
    {
        public void Info(string message) => Debug.Log($"[TM] {message}");
        public void Warning(string message) => Debug.LogWarning($"[TM] {message}");
        public void Error(string message) => Debug.LogError($"[TM] {message}");
    }
}
