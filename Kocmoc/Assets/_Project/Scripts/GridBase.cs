using UnityEngine;

namespace Kocmoc
{
    public class GridBase
    {
        public Vector2Int dimensions { get; private set; }
        public int cellCount {  get; private set; }
        public float cellSize;
        public bool centered;

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
            Vector2 cellPosition = (Vector2)limitedCoordinates * cellSize;
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
        public bool ValidateInput(int index) => index >= 0 && index < cellCount;
        public bool ValidateInput(Vector2Int coordinates) => coordinates.x >= 0 && coordinates.y >= 0 && coordinates.x < dimensions.x && coordinates.y < dimensions.y;
        public bool ValidateInput(int x, int y) => x >= 0 && y >= 0 && x < dimensions.x && y < dimensions.y;

        public bool LimitInput(int index, out int limitedIndex)
        {
            limitedIndex = Mathf.Clamp(index, 0, cellCount - 1);
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
        protected int CoordinatesToIndex(Vector2Int coordinates) => coordinates.y * dimensions.x + coordinates.x;
        protected int CoordinatesToIndex(int x, int y) => y * dimensions.x + x;
        protected Vector2 IndexToCoordinates(int index)
        {
            int row = index / dimensions.x;
            int col = index - dimensions.x * row;
            return new Vector2Int(col, row);
        }
        #endregion

        public GridBase(Vector2Int dimensions, float cellSize = 0, bool centered = false)
        {
            this.dimensions = dimensions;
            this.cellSize = cellSize;
            this.centered = centered;
            cellCount = dimensions.x * dimensions.y;
        }
    }
}
