using UnityEngine;

namespace Kocmoc
{
    public class Grid<T>
    {
        public Vector2Int dimensions { get; private set; }

        private T[] cells;

        public bool ValidateInput(int index) => index >= 0 && index < cells.Length;
        public bool ValidateInput(Vector2Int coordinates) => coordinates.x >= 0 && coordinates.y >= 0 && coordinates.x < dimensions.x && coordinates.y < dimensions.y;
        public bool ValidateInput(int x, int y) => x >= 0 && y >= 0 && x < dimensions.x && y < dimensions.y;

        private T GetCellRaw(int index) => cells[index];

        private int CoordinatesToIndex(Vector2Int coordinates) => coordinates.y * dimensions.x +  coordinates.x;
        private int CoordinatesToIndex(int x, int y) => y * dimensions.x + x;

        public Grid(Vector2Int dimensions)
        {
            this.dimensions = dimensions;
            cells = new T[dimensions.x * dimensions.y];
        }

        public Grid(Vector2Int dimensions, T defaultValue)
        {
            this.dimensions = dimensions;

            cells = new T[dimensions.x * dimensions.y];
            for (int i = 0; i < cells.Length; i++) cells[i] = defaultValue; 
        }
    }
}
