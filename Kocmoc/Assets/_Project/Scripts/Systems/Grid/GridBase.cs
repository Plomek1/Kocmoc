using System;
using UnityEngine;

namespace Kocmoc
{
    [System.Serializable]
    public class GridBase
    {
        public Vector2Int size { get; private set; }
        public Vector2 worldSize { get; private set; }
        public bool centered {  get; private set; }

        public int cellCount { get; private set; }
        public int firstCell { get; private set; }
        public int lastCell { get; private set; }
        public float cellSize { get; private set; }

        #region Cell position getters
        public Vector2 GetCellPosition(int index, bool centerOfCell = false) => GetCellPosition(IndexToCoordinates(index), centerOfCell);

        public Vector2 GetCellPosition(Vector2Int coordinates, bool centerOfCell = false)
        {
            Vector2 cellPosition = (Vector2)coordinates * cellSize;
            Vector2 halfCellSize = Vector2.one * cellSize * .5f;
            if (centered) cellPosition -= halfCellSize;
            return centerOfCell ? cellPosition + halfCellSize : cellPosition;
        }

        public Vector2 GetCellPositionLimited(int index, bool centerOfCell = false)
        {
            LimitInput(index, out int limitedIndex);
            return GetCellPosition(IndexToCoordinates(limitedIndex), centerOfCell);
        }

        public Vector2 GetCellPositionLimited(Vector2Int coordinates, bool centerOfCell = false)
        {
            LimitInput(coordinates, out Vector2Int limitedCoordinates);
            return GetCellPosition(limitedCoordinates, centerOfCell);
        }
        #endregion

        #region Parameter setters
        public void SetCellSize(float cellSize)
        {
            this.cellSize = cellSize;
            worldSize = (Vector2)size * cellSize;
        }

        public void SetCentered(bool centered)
        {
            this.centered = centered; 
            firstCell = centered ? CenterInput(0) : 0;
            lastCell = centered ? CenterInput(cellCount - 1) : cellCount - 1;
        }
        #endregion

        #region Input validation
        public bool ValidateInput(int index) => index >= 0 && index < cellCount;
        public bool ValidateInput(Vector2Int coordinates) => coordinates.x >= 0 && coordinates.y >= 0 && coordinates.x < size.x && coordinates.y < size.y;

        public bool LimitInput(int index, out int limitedIndex)
        {
            limitedIndex = Mathf.Clamp(index, 0, cellCount - 1);
            return index == limitedIndex;
        }

        public bool LimitInput(Vector2Int coordinates, out Vector2Int limitedCoordinates)
        {
            limitedCoordinates = Vector2Int.zero;
            limitedCoordinates.x = Mathf.Clamp(coordinates.x, 0, size.x - 1);
            limitedCoordinates.y = Mathf.Clamp(coordinates.y, 0, size.y - 1);
            return coordinates == limitedCoordinates;
        }
        #endregion

        #region Utility
        public int UncenterInput(int index) => index + Mathf.RoundToInt((float)(cellCount- 1) / 2);
        public Vector2Int UncenterInput(Vector2Int coordinates)
        {
            coordinates.x += Mathf.RoundToInt((float)(size.x - 1) / 2);
            coordinates.y += Mathf.RoundToInt((float)(size.y - 1) / 2);
            return coordinates;
        }

        public int CenterInput(int index) => index - Mathf.RoundToInt((float)(cellCount - 1) / 2);
        public Vector2Int CenterInput(Vector2Int coordinates)
        {
            coordinates.x -= Mathf.RoundToInt((float)(size.x - 1) / 2);
            coordinates.y -= Mathf.RoundToInt((float)(size.y - 1) / 2);
            return coordinates;
        }

        protected int CoordinatesToIndex(Vector2Int coordinates) => coordinates.y * size.x + coordinates.x;
        protected Vector2Int IndexToCoordinates(int index, bool alreadyUncenter = false)
        {
            if (centered && !alreadyUncenter) index = UncenterInput(index);
            int x = index % size.x;
            int y = index / size.x;
            return centered ? CenterInput(new Vector2Int(x, y)) : new Vector2Int(x, y);
        }
        #endregion

        public GridBase(Vector2Int size, float cellSize = 0, bool centered = false)
        {
            this.size = size;
            cellCount = size.x * size.y;

            SetCellSize(cellSize);
            SetCentered(centered);
        }
    }
}
