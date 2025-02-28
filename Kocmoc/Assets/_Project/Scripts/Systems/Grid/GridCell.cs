using UnityEngine;

namespace Kocmoc
{
    public class GridCell<T>
    {
        public T value { get; private set; }
        public int index { get; private set; }
        public Vector2Int coordinates { get; private set; }

        public bool inGroup => group != null;
        public bool isOrigin => coordinates == group.origin;
        public GridGroup group { get; private set; }

        public void SetData(T value)
        {
            this.value = value;
            group = null;
        }

        public void AddToGroup(T value, GridGroup group)
        {
            this.value = value;
            this.group = group;
        }

        public GridCell(int index, Vector2Int coordinates, T value = default, GridGroup group = null)
        {
            this.index = index;
            this.coordinates = coordinates;
            this.value = value;
            this.group = group;
        }
    }
}
