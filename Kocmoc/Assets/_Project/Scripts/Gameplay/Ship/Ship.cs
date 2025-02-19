using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ship : MonoBehaviour
    {
        public Action<ShipController> ControllerAttached;

        public ShipData shipData;

        [SerializeField] private Transform centerOfMass;
        [SerializeField] private Transform cellsRoot;

        private Rigidbody2D rb;
        private GridRenderer gridRenderer;

        public Transform GetCenterOfMass() => centerOfMass;

        public void Init(ShipData shipData)
        {
            this.shipData = shipData;
            shipData.MassUpdated += OnMassUpdate;

            foreach (GridCell<ShipCellData> cell in shipData.grid.GetCells())
            {
                ShipCell cellGo = Instantiate(cell.data.prefab, shipData.grid.GetCellPosition(cell.coordinates, centerOfCell: true), Quaternion.identity, cellsRoot);
                cellGo.Init(this, cell.data);
            }

            rb = GetComponent<Rigidbody2D>();
            OnMassUpdate();

            gridRenderer = GetComponentInChildren<GridRenderer>();
            gridRenderer.SetGrid(shipData.grid);
            gridRenderer.StartRendering();

        }

        public void AttachController(ShipControllerType type)
        {
            ShipController controller = null;
            switch (type)
            {
                case ShipControllerType.Player:
                    controller = transform.AddComponent<PlayerShipController>();
                    break;
            }

            ControllerAttached?.Invoke(controller);
        }

        private void OnMassUpdate()
        {

            Vector3 centerOfMassDelta = (Vector3)shipData.centerOfMass - centerOfMass.position;
            if (centerOfMassDelta.sqrMagnitude > 0)
            {
                centerOfMass.position = shipData.centerOfMass;

                rb.mass = shipData.totalMass;
                rb.inertia = shipData.totalMass;
                rb.centerOfMass = shipData.centerOfMass;
            }

        }
    }
}
