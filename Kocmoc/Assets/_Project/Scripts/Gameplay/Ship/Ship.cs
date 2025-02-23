using System;
using Unity.VisualScripting;
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

        public bool inMotion => rb.linearVelocity.magnitude > 0 || rb.angularVelocity != 0;
        public bool inCombat => false;

        [SerializeField] private Transform centerOfMass;
        [SerializeField] private Transform cellsRoot;

        private Rigidbody2D rb;

        public Transform GetCenterOfMass() => centerOfMass;

        public void Init(ShipData data, ShipType type)
        {
            this.data = data;
            this.type = type;
            AttachController(type);

            this.data.grid.SetOrigin(transform);
            this.data.MassUpdated += OnMassUpdate;

            foreach (GridCell<ShipCellData> cell in data.grid.GetCells())
            {
                ShipCell cellGo = Instantiate(cell.data.prefab, data.grid.GetCellPosition(cell.coordinates, centerOfCell: true), Quaternion.identity, cellsRoot);
                cellGo.Init(this, cell.data);
            }

            rb = GetComponent<Rigidbody2D>();
            OnMassUpdate();

            gridRenderer = GetComponentInChildren<GridRenderer>();
            gridRenderer.SetGrid(data.grid);
        }

        public void AttachController(ShipType type)
        {
            ShipController controller = null;
            switch (type)
            {
                case ShipType.Player:
                    controller = transform.AddComponent<PlayerShipController>();
                    break;
            }

            ControllerAttached?.Invoke(controller);
        }

        private void OnMassUpdate()
        {

            Vector3 centerOfMassDelta = (Vector3)data.centerOfMass - centerOfMass.position;
            if (centerOfMassDelta.sqrMagnitude > 0)
            {
                centerOfMass.position = data.centerOfMass;

                rb.mass = data.totalMass;
                rb.inertia = data.totalMass;
                rb.centerOfMass = data.centerOfMass;
            }
        }

        
    }
    public enum ShipType
    {
        Player,
        AI
    }
}
