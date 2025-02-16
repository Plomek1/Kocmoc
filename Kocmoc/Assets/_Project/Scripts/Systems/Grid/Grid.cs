using System;
using UnityEngine;

namespace Kocmoc
{
    public class Grid<T> : GridBase
    {
        private GridCell<T>[] cells;

        #region Cell setters
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
            if (centered) index= UncenterInput(index);
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
        private T GetCellRaw(int index) => cells[index].data;
        private T GetCellRaw(Vector2Int coordinates) => cells[CoordinatesToIndex(coordinates)].data;

        private void SetCellRaw(int index, T value) => cells[index].SetData(value);
        private void SetCellRaw(Vector2Int coordinates, T value) => cells[CoordinatesToIndex(coordinates)].SetData(value);
        #endregion

        public Grid(Vector2Int size, T startingValue = default, float cellSize = 1, bool centered = false) : base(size, cellSize, centered)
        {
            cells = new GridCell<T>[size.x * size.y];
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    Vector2Int coordinates = new Vector2Int(x, y);
                    cells[CoordinatesToIndex(coordinates)] = new GridCell<T>(coordinates, startingValue);
                }
            }
        }
    }
}
