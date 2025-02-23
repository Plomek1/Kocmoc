using System.Collections.Generic;
using UnityEngine;

namespace Kocmoc
{
    public class Grid<T> : GridBase
    {
        private bool[] occupied;
        private Dictionary<int, GridCell<T>> cells;

        #region Cell setters
        public ICollection<GridCell<T>> GetCells() => cells.Values;

        public void SetCell(int index, T value)
        {
            if (centered) index = UncenterInput(index);
            if (!ValidateInput(index)) return;
            SetCellRaw(index, value);
        }

        public void SetCell(Vector2Int coordinates, T value)
        {
            if (centered) coordinates = UncenterInput(coordinates);
            if (!ValidateInput(coordinates)) return;
            SetCellRaw(coordinates, value);
        }

        public void SetCellLimited(int index, T value)
        {
            if (centered) index = UncenterInput(index);
            LimitInput(index, out int limitedIndex);
            SetCellRaw(limitedIndex, value);
        }

        public void SetCellLimited(Vector2Int coordinates, T value)
        {
            if (centered) coordinates = UncenterInput(coordinates);
            LimitInput(coordinates, out Vector2Int limitedCoordinates);
            SetCellRaw(limitedCoordinates, value);
        }
        #endregion

        #region Cell getters
        public T GetCell(int index)
        {
            if (centered) index = UncenterInput(index);
            if (!ValidateInput(index)) return default;
            return GetCellRaw(index);
        }

        public T GetCell(Vector2Int coordinates)
        {
            if (centered) coordinates = UncenterInput(coordinates);
            if (!ValidateInput(coordinates)) return default;
            return GetCellRaw(coordinates);
        }

        public T GetCellLimited(int index)
        {
            if (centered) index = UncenterInput(index);
            LimitInput(index, out int limitedIndex);
            return GetCellRaw(limitedIndex);
        }

        public T GetCellLimited(Vector2Int coordinates)
        {
            if (centered) coordinates = UncenterInput(coordinates);
            LimitInput(coordinates, out Vector2Int limitedCoordinates);
            return GetCellRaw(coordinates);
        }
        #endregion

        #region Raw getters and setters
        private T GetCellRaw(int index) => occupied[index] ? cells[index].data : default;
        private T GetCellRaw(Vector2Int coordinates) => GetCellRaw(CoordinatesToIndex(coordinates));

        private void SetCellRaw(int index, T value)
        {
            if (object.Equals(value, default(T)) && occupied[index])
            {
                cells.Remove(index);
                occupied[index] = false;
                return;
            }

            if (occupied[index])
            {
                cells[index].SetData(value);
                return;
            }
            Vector2Int coordinates = IndexToCoordinates(index, alreadyUncenter: true);
            cells.Add(index, new GridCell<T>(coordinates, value));
            occupied[index] = true;
        }
        private void SetCellRaw(Vector2Int coordinates, T value) => SetCellRaw(CoordinatesToIndex(coordinates), value);
        #endregion

        public bool IsOccupied(int index) => occupied[index];
        public bool IsOccupied(Vector2Int coordinates) => occupied[CoordinatesToIndex(coordinates)];

        public Grid (Vector2Int size, Transform origin = null, float cellSize = 1, bool centered = false) : base(size, origin, cellSize, centered)
        {
            cells = new Dictionary<int, GridCell<T>>();
            occupied = new bool[size.x * size.y];
        }

        public Grid (Vector2Int size, T startingValue, Transform origin = null, float cellSize = 1, bool centered = false) : base(size, origin, cellSize, centered)
        {
            cells = new Dictionary<int, GridCell<T>>(size.x * size.y);
            occupied = new bool[size.x * size.y];

            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    Vector2Int coordinates = new Vector2Int(x, y);
                    int index = CoordinatesToIndex(coordinates);
                    cells.Add(index, new GridCell<T>(coordinates, startingValue));
                    occupied[index] = true;
                }
            }
        }
    }
}
