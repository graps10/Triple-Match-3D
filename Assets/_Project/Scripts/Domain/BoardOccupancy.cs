using System.Collections.Generic;

namespace TripleMatch.Domain
{
    public class BoardOccupancy
    {
        private readonly Dictionary<(float X, float Y), int> _topLayerByPosition = new();

        public void Register(float x, float y, int layer)
        {
            (float x, float y) key = (x, y);
            if (!_topLayerByPosition.TryGetValue(key, out int current) || layer > current)
                _topLayerByPosition[key] = layer;
        }

        public bool IsTopmost(float x, float y, int layer) =>
            _topLayerByPosition.TryGetValue((x, y), out int top) && layer == top;

        // Call once the topmost item at this position has been removed, revealing the
        // layer below it.
        public void Remove(float x, float y, int layer)
        {
            (float x, float y) key = (x, y);
            if (_topLayerByPosition.TryGetValue(key, out int top) && top == layer)
                _topLayerByPosition[key] = layer - 1;
        }

        // Used when every tracked position is about to be reassigned at once (Shuffle) -
        // cheaper and clearer than calling Remove for each old position individually.
        public void Clear() => _topLayerByPosition.Clear();
    }
}
