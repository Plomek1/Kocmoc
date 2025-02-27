using System;
using UnityEngine;

namespace Kocmoc
{
    [System.Serializable]
    public class GridBase
    {
        public Action GridUpdated;

        public Transform origin { get; private set; }

        public Vector2Int size { get; private set; }
        public Vector2 worldSize { get; private set; }
        public bool centered { get; private set; }

        public int cellCount { get; private set; }
        public int firstCell { get; private set; }
        public int lastCell { get; private set; }
        public float cellSize { get; private set; }

        #region Cell position getters
        public bool IsInGroup(Vector2Int coordinates, out GridGroup group) => IsInGroup(CoordinatesToIndex(coordinates), out group);
        public virtual bool IsInGroup(int index, out GridGroup gridGroup)
        {
            gridGroup = null;
            return false;
        }

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

        public Vector2 GetCellWorldPosition(int index, bool centerOfCell = false) => ApplyOriginTransform(GetCellPosition(index, centerOfCell));
        public Vector2 GetCellWorldPosition(Vector2Int coordinates, bool centerOfCell = false) => ApplyOriginTransform(GetCellPosition(coordinates, centerOfCell));

        public Vector2 GetCellWorldPositionLimited(int index, bool centerOfCell = false) => ApplyOriginTransform(GetCellPositionLimited(index, centerOfCell));
        public Vector2 GetCellWorldPositionLimited(Vector2Int coordinates, bool centerOfCell = false) => ApplyOriginTransform(GetCellPositionLimited(coordinates, centerOfCell));

        public Vector2Int PositionToCell(Vector2 position)
        {
            if (origin)
            {
                position -= (Vector2)origin.position;
                float angle = -Vector2.SignedAngle(Vector2.up, origin.up);
                position = Quaternion.AngleAxis(angle, Vector3.forward) * position;
            }

            int x = Mathf.RoundToInt(position.x / cellSize);
            int y = Mathf.RoundToInt(position.y / cellSize);

            return new Vector2Int(x, y);
        }

        public Vector2Int PositionToCellLimited(Vector2 position)
        {
            LimitInput(PositionToCell(position), out Vector2Int limitedCoordinates);
            return limitedCoordinates;
        }

        #endregion

        #region Cell groups
        public Vector2 GetGroupCenterCoordinates(GridGroup group)
        {
            Vector2 center = group.origin + (Vector2)(group.size - Vector2.one) * .5f;
            if (group.size.x < 0) center.x += 1;
            if (group.size.y < 0) center.y += 1;
            Debug.Log(center);
            return center;
        }

        public Vector2 GetGroupCenterPosition(GridGroup group) => GetGroupCenterCoordinates(group) * cellSize;
        public Vector2 GetGroupCenterWorldPosition(GridGroup group) => ApplyOriginTransform(GetGroupCenterPosition(group) * cellSize);
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

        public bool ValidateInput(int index)
        {
            if (centered) index = UncenterInput(index);
            return ValidateInputUncentered(index);
        }
        public bool ValidateInput(Vector2Int coordinates)
        {
            if (centered) coordinates = UncenterInput(coordinates);
            return ValidateInputUncentered(coordinates);
        }

        protected bool ValidateInputUncentered(int index) => index >= 0 && index < cellCount;
        protected bool ValidateInputUncentered(Vector2Int coordinates) => coordinates.x >= 0 && coordinates.y >= 0 && coordinates.x < size.x && coordinates.y < size.y;

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
        protected Vector2Int IndexToCoordinates(int index, bool alreadyUncentered = false)
        {
            if (centered && !alreadyUncentered) index = UncenterInput(index);
            int x = index % size.x;
            int y = index / size.x;
            return centered ? CenterInput(new Vector2Int(x, y)) : new Vector2Int(x, y);
        }

        private Vector2 ApplyOriginTransform(Vector2 position)
        {
            if (origin)
            {
                float angle = Vector2.SignedAngle(Vector2.up, origin.up);
                Vector2 cellPosition = Quaternion.AngleAxis(angle, Vector3.forward) * position;
                return (Vector2)origin.position + cellPosition;
            }
            return position;
        }
        #endregion

        public void SetOrigin(Transform origin) => this.origin = origin;

        public GridBase(Vector2Int size, Transform origin = null, float cellSize = 0, bool centered = false)
        {
            this.size = size;
            this.origin = origin;
            cellCount = size.x * size.y;

            SetCellSize(cellSize);
            SetCentered(centered);
        }
    }
}
