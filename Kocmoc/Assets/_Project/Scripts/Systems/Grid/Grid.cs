using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Kocmoc
{
    public class Grid<T> : GridBase
    {
        private bool[] occupied;
        private Dictionary<int, GridCell<T>> cells;

        #region Cell setters

        public bool SetCell(int index, T value)
        {
            if (centered) index = UncenterInput(index);
            if (!ValidateInputUncentered(index)) return false;
            SetCellRaw(index, value);
            return true;
        }

        public bool SetCell(Vector2Int coordinates, T value)
        {
            if (centered) coordinates = UncenterInput(coordinates);
            if (!ValidateInputUncentered(coordinates)) return false;
            SetCellRaw(coordinates, value);
            return true;
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
        public ICollection<GridCell<T>> GetCells() => cells.Values;

        public GridCell<T> GetCell(int index)
        {
            if (centered) index = UncenterInput(index);
            if (!ValidateInputUncentered(index)) return default;
            return GetCellRaw(index);
        }

        public GridCell<T> GetCell(Vector2Int coordinates)
        {
            if (centered) coordinates = UncenterInput(coordinates);
            if (!ValidateInputUncentered(coordinates)) return default;
            return GetCellRaw(coordinates);
        }

        public GridCell<T> GetCellLimited(int index)
        {
            if (centered) index = UncenterInput(index);
            LimitInput(index, out int limitedIndex);
            return GetCellRaw(limitedIndex);
        }

        public GridCell<T> GetCellLimited(Vector2Int coordinates)
        {
            if (centered) coordinates = UncenterInput(coordinates);
            LimitInput(coordinates, out Vector2Int limitedCoordinates);
            return GetCellRaw(coordinates);
        }
        #endregion

        #region Cell groups
        public bool CreateGroup(Vector2Int origin, Vector2Int size, T value, bool checkBounds = true)
        {
            if (size == Vector2Int.one)
            {
                SetCell(origin, value);
                return true;
            }

            if (checkBounds && !GroupInBounds(origin, size)) return false;
            GridGroup group = new GridGroup(origin, size);

            foreach (Vector2Int cell in group.occupiedCells)
            {
                if (centered)
                {
                    Vector2Int uncenteredCell = UncenterInput(cell);
                    SetCellRaw(uncenteredCell, value, group: group);
                    continue;
                }
                SetCellRaw(cell, value, group: group);
            }

            return true;
        }

        public void RemoveGroup(Vector2Int coordinates)
        {
            if (IsInGroup(coordinates, out GridGroup group))
            {
                foreach (Vector2Int cell in group.occupiedCells)
                {
                    if (centered)
                    {
                        Vector2Int uncenteredCell = UncenterInput(cell);
                        SetCellRaw(uncenteredCell, default);
                        continue;
                    }
                    SetCellRaw(cell, default);
                }
            }
        }

        public bool GroupInBounds(GridGroup group)
        {
            if (size == Vector2Int.one) return ValidateInput(group.origin);

            foreach (Vector2Int cell in group.occupiedCells)
                if (!ValidateInput(cell)) return false;
            return true;
        }


        public bool GroupInBounds(Vector2Int origin, Vector2Int size)
        {
            if (size == Vector2Int.one) return ValidateInput(origin);

            for (int y = 0; y < Mathf.Abs(size.y); y++)
            {
                for (int x = 0; x < Mathf.Abs(size.x); x++)
                {
                    Vector2Int coordinates = origin + new Vector2Int(x * (int)Mathf.Sign(size.x), y * (int)Mathf.Sign(size.y));
                    if (!ValidateInput(coordinates)) return false;
                }
            }

            return true;
        }

        public override bool IsInGroup(int index, out GridGroup group)
        {
            GridCell<T> cell = GetCell(index);
            if (cell == null)
            {
                group = null;
                return false;
            }

            group = cell.group;
            return cell.inGroup;
        }
        #endregion

        #region Raw getters and setters
        private GridCell<T> GetCellRaw(int index) => occupied[index] ? cells[index]: null;
        private GridCell<T> GetCellRaw(Vector2Int coordinates) => GetCellRaw(CoordinatesToIndex(coordinates));

        private void SetCellRaw(Vector2Int coordinates, T value, GridGroup group = null) => SetCellRaw(CoordinatesToIndex(coordinates), value, group);
        private void SetCellRaw(int index, T value, GridGroup group = null)
        {
            if (object.Equals(value, default(T)) )
            {
                if (occupied[index])
                {
                    cells.Remove(index);
                    occupied[index] = false;
                    GridUpdated?.Invoke();
                    Debug.Log(IndexToCoordinates(index, alreadyUncentered: true));

                }
                return;
            }

            if (occupied[index])
            {
                if (group != null) cells[index].AddToGroup(value, group);
                cells[index].SetData(value);
                GridUpdated?.Invoke();
                Debug.Log(IndexToCoordinates(index, alreadyUncentered: true));
                return;
            }

            Vector2Int coordinates = IndexToCoordinates(index, alreadyUncentered: true);

            cells.Add(index, new GridCell<T>(coordinates, value, group));
            occupied[index] = true;

            GridUpdated?.Invoke();
        }
        #endregion

        public bool IsOccupied(Vector2Int coordinates) => IsOccupied(CoordinatesToIndex(coordinates));
        public bool IsOccupied(int index)
        {
            if (centered) index = UncenterInput(index);
            return occupied[index];
        }

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

    public class GridGroup
    {
        public Vector2Int origin;
        public Vector2Int size;
        public Vector2Int sizeAbs;

        public List<Vector2Int> occupiedCells; 

        public GridGroup(Vector2Int origin, Vector2Int size)
        {
            this.origin = origin;
            this.size = size;
            sizeAbs = new Vector2Int(Mathf.Abs(size.x), Mathf.Abs(size.y));

            occupiedCells = new List<Vector2Int>(sizeAbs.x * sizeAbs.y);

            for (int y = 0; y < sizeAbs.y; y++)
            {
                for (int x = 0; x < sizeAbs.x; x++)
                {
                    Vector2Int coordinates = this.origin + new Vector2Int(x * (int)Mathf.Sign(size.x), y * (int)Mathf.Sign(size.y));
                    occupiedCells.Add(coordinates);
                }
            }

        }
    }
}
