using System;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class ShipData : ScriptableObject
    {
        public Action CenterOfMassChanged;

        public Grid<ShipCellData> grid {  get; private set; }

        public Vector2 centerOfMass { get; private set; }
        public float totalMass { get; private set; }
        public float rotationAcceleration { get; private set; } = .05f;

        private void UpdateCenterOfMass()
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
            CenterOfMassChanged?.Invoke();
        }

        public void Init(Grid<ShipCellData> grid)
        {
            this.grid = grid;
            UpdateCenterOfMass();
        }
    }
}
