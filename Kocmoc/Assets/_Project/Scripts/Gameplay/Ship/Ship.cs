using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ship : MonoBehaviour
    {
        public Action<ShipController> ControllerAttached;

        public ShipData data { get; private set; }
        public ShipType type { get; private set; }

        public GridRenderer gridRenderer { get; private set; }
        public GridSelector gridSelector { get; private set; }

        public bool inMotion => rb.linearVelocity.magnitude > 0 || rb.angularVelocity != 0;
        public bool inCombat => false;

        [SerializeField] private Transform centerOfMass;
        [SerializeField] private Transform cellsRoot;

        private Rigidbody2D rb;
        private Dictionary<int, ShipCell> cells;

        public Transform GetCenterOfMass() => centerOfMass;

        public void Init(ShipData data, ShipType type)
        {
            this.data = data;
            this.type = type;
            AttachController(type);

            this.data.grid.SetOrigin(transform);
            this.data.MassUpdated += OnMassUpdate;

            cells = new(data.grid.GetCells().Count);
            foreach (GridCell<ShipCellData> cell in data.grid.GetCells())
            {
                if (cell.inGroup && !cell.isOrigin) continue;
                SpawnCell(cell.value);
            }

            rb = GetComponent<Rigidbody2D>();
            OnMassUpdate();

            gridRenderer = GetComponentInChildren<GridRenderer>();
            gridSelector = GetComponentInChildren<GridSelector>();
            gridRenderer.SetGrid(data.grid);
            gridSelector.SetGrid(data.grid);
        }

        public void AddCell(ShipCellData cellData)
        {
            data.AddCell(cellData);
            SpawnCell(cellData);
        }

        public void RemoveCell(Vector2Int cellCoordinates)
        {
            if (data.grid.IsInGroup(cellCoordinates, out GridGroup group)) cellCoordinates = group.origin;
            data.RemoveCell(cellCoordinates);
            int cellIndex = data.grid.CoordinatesToIndex(cellCoordinates);
            Destroy(cells[cellIndex].gameObject);
            cells.Remove(cellIndex);
        }

        public void AttachController(ShipType type)
        {
            ShipController controller = null;
            switch (type)
            {
                case ShipType.Player:
                    controller = transform.gameObject.AddComponent<PlayerShipController>();
                    break;
            }

            ControllerAttached?.Invoke(controller);
        }

        private void SpawnCell(ShipCellData cellData)
        {
            Vector2 cellLocalPosition = data.grid.GetCellPosition(cellData.coordinates, centerOfCell: true);
            ShipCell cellGo = Instantiate(cellData.prefab, cellsRoot);
            cellGo.transform.localPosition = cellLocalPosition;
            cellGo.Init(this, cellData);
            cells.Add(data.grid.CoordinatesToIndex(cellData.coordinates), cellGo);
        }

        private void OnMassUpdate()
        {
            Vector3 centerOfMassDelta = (Vector3)data.centerOfMass - centerOfMass.position;
            if (centerOfMassDelta.sqrMagnitude > 0)
            {
                centerOfMass.localPosition = data.centerOfMass;

                rb.mass = data.totalMass;
                rb.inertia = data.totalMass;
                rb.centerOfMass = data.centerOfMass;
            }
        }
    }

    public enum ShipType
    {
        Wreck,
        Player,
        AI
    }
}
