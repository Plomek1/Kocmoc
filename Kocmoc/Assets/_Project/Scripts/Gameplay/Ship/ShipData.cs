using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class ShipData : ScriptableObject
    {
        public Action MassUpdated;

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

        public Rotation GetStrongestThrustDirection()
        {
            var strongestThrust = thrustForces.Aggregate((a, b) => a.Value > b.Value ? a : b);
            return strongestThrust.Value > thrustForces[Rotation.Up] ? strongestThrust.Key : Rotation.Up;
        }

        private void UpdateMass()
        {
            Vector2 newCenterOfMass = Vector2.zero;
            totalMass = 0;

            foreach (var cell in grid.GetCells())
            {
                float cellMass = cell.data.mass;
                newCenterOfMass += grid.GetCellPosition(cell.coordinates, centerOfCell: true) * cellMass;
                totalMass += cellMass;
            }

            centerOfMass = totalMass != 0 ? newCenterOfMass / totalMass : Vector2.zero;
            MassUpdated?.Invoke();
        }

        public void AddCell(ShipCellData cellData)
        {
            grid.SetCell(cellData.coordinates, cellData);
            UpdateMass();
        }

        public void RemoveCell(Vector2Int cellCoordinates)
        {
            grid.SetCell(cellCoordinates, null);
            UpdateMass();
        }

        public void Init(Grid<ShipCellData> grid)
        {
            this.grid = grid;
            foreach (var cell in grid.GetCells()) cell.data.SetShip(this);

            UpdateMass();
        }
    }
}
