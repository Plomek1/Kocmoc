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

        #region Cell position getters
        public Vector2 GetGridPosition(int index, bool centered = false)
        {
            if (!ValidateInput(index)) return Vector2.negativeInfinity;
            Vector2 cellPosition = IndexToCoordinates(index) * cellSize;
            return centered ? cellPosition + Vector2.one * cellSize * .5f : cellPosition;
        }

        public Vector2 GetGridPosition(Vector2Int coordinates, bool centered = false)
        {
            if (!ValidateInput(coordinates)) return Vector2.negativeInfinity;
            Vector2 cellPosition = (Vector2)coordinates * cellSize;
            return centered ? cellPosition + Vector2.one * cellSize * .5f : cellPosition;
        }

        public Vector2 GetGridPosition(int x, int y, bool centered = false)
        {
            if (!ValidateInput(x, y)) return Vector2.negativeInfinity;
            Vector2 cellPosition = new Vector2(x, y) * cellSize;
            return centered ? cellPosition + Vector2.one * cellSize * .5f : cellPosition;
        }

        public Vector2 GetGridPositionLimited(int index, bool centered = false)
        {
            LimitInput(index, out int limitedIndex);
            Vector2 cellPosition = IndexToCoordinates(limitedIndex) * cellSize;
            return centered ? cellPosition + Vector2.one * cellSize * .5f : cellPosition;
        }

        public Vector2 GetGridPositionLimited(Vector2Int coordinates, bool centered = false)
        {
            LimitInput(coordinates, out Vector2Int limitedCoordinates);
            Vector2 cellPosition = (Vector2)limitedCoordinates* cellSize;
            return centered ? cellPosition + Vector2.one * cellSize * .5f : cellPosition;
        }

        public Vector2 GetGridPositionLimited(int x, int y, bool centered = false)
        {
            LimitInput(x, y, out int limitedX, out int limitedY);
            Vector2 cellPosition = new Vector2(limitedX, limitedY) * cellSize;
            return centered ? cellPosition + Vector2.one * cellSize * .5f : cellPosition;
        }
        #endregion

        #region Input validation
        public bool ValidateInput(int index) => index >= 0 && index < cells.Length;
        public bool ValidateInput(Vector2Int coordinates) => coordinates.x >= 0 && coordinates.y >= 0 && coordinates.x < dimensions.x && coordinates.y < dimensions.y;
        public bool ValidateInput(int x, int y) => x >= 0 && y >= 0 && x < dimensions.x && y < dimensions.y;

        public bool LimitInput(int index, out int limitedIndex)
        {
            limitedIndex = Mathf.Clamp(index, 0, cells.Length - 1);
            return index == limitedIndex;
        }

        public bool LimitInput(Vector2Int coordinates, out Vector2Int limitedCoordinates)
        {
            limitedCoordinates = Vector2Int.zero;
            limitedCoordinates.x = Mathf.Clamp(coordinates.x, 0, dimensions.x - 1);
            limitedCoordinates.y = Mathf.Clamp(coordinates.y, 0, dimensions.y - 1);
            return coordinates == limitedCoordinates;
        }

        public bool LimitInput(int x, int y, out int limitedX, out int limitedY)
        {
            limitedX = Mathf.Clamp(x, 0, dimensions.x - 1);
            limitedY = Mathf.Clamp(y, 0, dimensions.y - 1);
            return limitedX == x && limitedY == y;
        }
        #endregion

        #region Utility
        private T GetCellRaw(int index) => cells[index];
        private T GetCellRaw(Vector2Int coordinates) => cells[CoordinatesToIndex(coordinates)];
        private T GetCellRaw(int x, int y) => cells[CoordinatesToIndex(x, y)];

        private void SetCellRaw(int index, T value) => cells[index] = value;
        private void SetCellRaw(Vector2Int coordinates, T value) => cells[CoordinatesToIndex(coordinates)] = value;
        private void SetCellRaw(int x, int y, T value) => cells[CoordinatesToIndex(x, y)] = value;

        private int CoordinatesToIndex(Vector2Int coordinates) => coordinates.y * dimensions.x +  coordinates.x;
        private int CoordinatesToIndex(int x, int y) => y * dimensions.x + x;
        private Vector2 IndexToCoordinates(int index)
        {
            int row = index / dimensions.x;
            int col = index - dimensions.x * row;
            return new Vector2Int(row, col);
        }
        #endregion

        public Grid(Vector2Int dimensions, float cellSize = 0) : base(dimensions, cellSize)
        {
            cells = new T[dimensions.x * dimensions.y];
        }

        public Grid(Vector2Int dimensions, T defaultValue, float cellSize = 0) : base(dimensions, cellSize)
        {
            cells = new T[dimensions.x * dimensions.y];
            for (int i = 0; i < cells.Length; i++) cells[i] = defaultValue; 
        }
    }
}
