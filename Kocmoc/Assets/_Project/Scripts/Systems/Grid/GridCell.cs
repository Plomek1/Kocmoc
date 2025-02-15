using UnityEngine;

namespace Kocmoc
{
    public class GridCell<T>
    {
        public T data { get; private set; }
        public Vector2Int coordinates { get; private set; }

        public void SetData(T data) => this.data = data;

        public GridCell(Vector2Int coordinates, T value = default)
        {
            this.data = value;
            this.coordinates = coordinates;
        }
    }
}
