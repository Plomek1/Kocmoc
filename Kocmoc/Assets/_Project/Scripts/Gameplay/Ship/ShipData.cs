using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class ShipData
    {
        public Action MassUpdated;

        public string name {  get; private set; }

        public Grid<ShipCellData> grid {  get; private set; }

        public Vector2 centerOfMass { get; private set; }
        public float totalMass { get; private set; }
        public float angularAcceleration { get; private set; } = 25f;

        public Dictionary<Rotation, float> thrustForces { get; private set; } = new Dictionary<Rotation, float>(4) 
        { 
            { Rotation.Up, 0f },
            { Rotation.Right, 0f },
            { Rotation.Down, 0f },
            { Rotation.Left, 0f },
        };

        public void AddCell(ShipCellData cellData)
        {
            Vector2Int rotatedSize = cellData.size.RightAngleRotate(cellData.currentRotation);
            grid.CreateGroup(cellData.coordinates, rotatedSize, cellData, checkBounds: false);
            UpdateMass();
        }

        public void RemoveCell(Vector2Int cellCoordinates)
        {
            if (grid.IsInGroup(cellCoordinates, out GridGroup group))
                grid.RemoveGroup(group.origin);
            else
                grid.SetCell(cellCoordinates, null);
            
            UpdateMass();
        }

        public HashSet<int> GetConnectedCells(int? indexToIgnore = null)
        {
            HashSet<int> visited = new();
            Queue<int> cellsQueue = new();
            
            visited.Add(0);
            cellsQueue.Enqueue(0);

            while (cellsQueue.Count > 0)
            {
                int cellIndex = cellsQueue.Dequeue();
                GridCell<ShipCellData> cell = grid.GetCell(cellIndex);
                HashSet<Vector2Int> occupiedCells = cell.inGroup ? cell.group.occupiedCells : new() { cell.coordinates };

                foreach (Vector2Int connectionPoint in cell.value.connectionPoints)
                {
                    int connectionPointIndex = grid.CoordinatesToIndex(connectionPoint);
                    
                    GridCell<ShipCellData> neighbourCell = grid.GetCell(connectionPointIndex);
                    if (neighbourCell == null) continue;
                    if (grid.IsInGroup(connectionPointIndex, out GridGroup neighbourGroup))
                        connectionPointIndex = grid.CoordinatesToIndex(neighbourGroup.origin);

                    if (indexToIgnore.HasValue && indexToIgnore.Value == connectionPointIndex) 
                        continue; //Neighbour cell is the cell to ignore

                    if (occupiedCells.Intersect(neighbourCell.value.connectionPoints).Count() == 0)
                        continue; //Neighbour isnt connected to the cell

                    if (!visited.Contains(connectionPointIndex) && grid.IsOccupied(connectionPointIndex))
                    {
                        visited.Add(connectionPointIndex);
                        cellsQueue.Enqueue(connectionPointIndex);
                    }
                }
            }

            return visited;
        }

        public HashSet<int> GetDanglingCells(int? indexToIgnore = null)
        {
            HashSet<int> danglingCells = new();
            HashSet<int> connectedCells = GetConnectedCells(indexToIgnore);

            foreach (GridCell<ShipCellData> cell in grid.GetCells())
            {
                if (cell.inGroup && !cell.isOrigin) continue;
                if (indexToIgnore.HasValue && cell.index == indexToIgnore.Value) continue;
                
                if (!connectedCells.Contains(cell.index))
                    danglingCells.Add(cell.index);
            }
            return danglingCells;
        }

        public bool CanPlaceCell(ShipCellData cell)
        {
            HashSet<Vector2Int> occupiedCells;

            if (cell.size == Vector2Int.one)
                occupiedCells = new() { cell.coordinates };
            else
            {
                occupiedCells = new(cell.size.x * cell.size.y);
                Vector2Int rotatedSize = cell.size.RightAngleRotate(cell.currentRotation);

                for (int y = 0; y < Mathf.Abs(rotatedSize.y); y++)
                {
                    for (int x = 0; x < Mathf.Abs(rotatedSize.x); x++)
                    {
                        Vector2Int cellCoordinates = cell.coordinates + new Vector2Int(x * (int)Mathf.Sign(rotatedSize.x), y * (int)Mathf.Sign(rotatedSize.y));
                        if (!grid.ValidateInput(cellCoordinates)) return false;
                        occupiedCells.Add(cellCoordinates);
                    }
                }
            }

            //Checking if the space cell wants to be in is free
            foreach (Vector2Int occupiedCell in occupiedCells)
                if (grid.IsOccupied(occupiedCell)) return false;

            //Checking if there is any cell that this cell can connect to
            bool canConnect = false;
            foreach (Vector2Int connectionPoint in cell.connectionPoints)
            {
                var neighbourCell = grid.GetCell(connectionPoint);
                if (neighbourCell == null) continue;

                if (occupiedCells.Intersect(neighbourCell.value.connectionPoints).Count() > 0)
                {
                    canConnect = true;
                    break;
                }
            }
            if (!canConnect) return false;

            return true;
        }

        private void UpdateMass()
        {
            Vector2 newCenterOfMass = Vector2.zero;
            totalMass = 0;

            foreach (var cell in grid.GetCells())
            {
                float cellMass = cell.value.mass;

                if (cell.inGroup)
                {
                    if (!cell.isOrigin) continue;
                    newCenterOfMass += grid.GetGroupCenterPosition(cell.group) * cellMass;
                    totalMass += cellMass;
                }
                else
                {
                    newCenterOfMass += grid.GetCellPosition(cell.coordinates, centerOfCell: true) * cellMass;
                    totalMass += cellMass;
                }
            }

            centerOfMass = totalMass != 0 ? newCenterOfMass / totalMass : Vector2.zero;
            MassUpdated?.Invoke();
        }

        public Rotation GetStrongestThrustDirection()
        {
            var strongestThrust = thrustForces.Aggregate((a, b) => a.Value > b.Value ? a : b);
            return strongestThrust.Value > thrustForces[Rotation.Up] ? strongestThrust.Key : Rotation.Up;
        }

        public ShipData(string name, Grid<ShipCellData> grid)
        {
            this.name = name;
            this.grid = grid;
            foreach (var cell in grid.GetCells()) 
                cell.value.SetShip(this);

            UpdateMass();
        }
    }
}
