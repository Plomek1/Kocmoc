using JetBrains.Annotations;
using Mono.Cecil;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Kocmoc
{
    public class Grid<T> : GridBase
    {
        private T[] cells;

        #region Cell setters
        public void SetCell(int index, T value)
        {
            if (!ValidateInput(index)) return;
            SetCellRaw(index, value);
        }

        public void SetCell(Vector2Int coordinates, T value)
        {
            if (!ValidateInput(coordinates)) return;
            SetCellRaw(coordinates, value);
        }

        public void SetCell(int x, int y, T value)
        {
            if (!ValidateInput(x, y)) return;
            SetCellRaw(x, y, value);
        }

        public void SetCellLimited(int index, T value)
        {
            LimitInput(index, out int limitedIndex);
            SetCellRaw(limitedIndex, value);
        }

        public void SetCellLimited(Vector2Int coordinates, T value)
        {
            LimitInput(coordinates, out Vector2Int limitedCoordinates);
            SetCellRaw(limitedCoordinates, value);
        }

        public void SetCellLimited(int x, int y, T value)
        {
            LimitInput(x, y, out int limitedX, out int limitedY);
            SetCellRaw(x, y, value);
        }
        #endregion

        #region Cell getters
        public T GetCell(int index)
        {
            if (!ValidateInput(index)) return default;
            return GetCellRaw(index);
        }

        public T GetCell(Vector2Int coordinates)
        {
            if (!ValidateInput(coordinates)) return default;
            return GetCellRaw(coordinates);
        }

        public T GetCell(int x, int y)
        {
            if (!ValidateInput(x, y)) return default;
            return GetCellRaw(x, y);
        }

        public T GetCellLimited(int index)
        {
            LimitInput(index, out int limitedIndex);
            return GetCellRaw(limitedIndex);
        }

        public T GetCellLimited(Vector2Int coordinates)
        {
            LimitInput(coordinates, out Vector2Int limitedCoordinates);
            return GetCellRaw(coordinates);
        }

        public T GetCellLimited(int x, int y)
        {
            LimitInput(x, y, out int limitedX, out int limitedY);
            return GetCellRaw(x, y);
        }
        #endregion

        #region Raw getters and setters
        private T GetCellRaw(int index) => cells[index];
        private T GetCellRaw(Vector2Int coordinates) => cells[CoordinatesToIndex(coordinates)];
        private T GetCellRaw(int x, int y) => cells[CoordinatesToIndex(x, y)];

        private void SetCellRaw(int index, T value) => cells[index] = value;
        private void SetCellRaw(Vector2Int coordinates, T value) => cells[CoordinatesToIndex(coordinates)] = value;
        private void SetCellRaw(int x, int y, T value) => cells[CoordinatesToIndex(x, y)] = value;
        #endregion
        //TODO: CENTERING COORDINATES
        public Grid(Vector2Int dimensions, float cellSize = 0, bool centered = false) : base(dimensions, cellSize, centered)
        {
            cells = new T[dimensions.x * dimensions.y];
        }

        public Grid(Vector2Int dimensions, T defaultValue, float cellSize = 0, bool centered = false) : base(dimensions, cellSize, centered)
        {
            cells = new T[dimensions.x * dimensions.y];
            for (int i = 0; i < cells.Length; i++) cells[i] = defaultValue; 
        }
    }
}
