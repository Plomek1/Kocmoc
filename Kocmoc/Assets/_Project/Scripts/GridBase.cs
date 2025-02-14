using UnityEngine;

namespace Kocmoc
{
    public class GridBase
    {
        public Vector2Int dimensions { get; private set; }
        public float cellSize;

        public GridBase(Vector2Int dimensions, float cellSize)
        {
            this.dimensions = dimensions;
            this.cellSize = cellSize;
        }
    }
}
