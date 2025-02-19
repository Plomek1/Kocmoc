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

            Camera.main.GetComponent<CameraDrag>().SetTarget(transform);
        }

        public void AttachController(ShipControllerType type)
        {
            ShipController controller = null;
            switch (type)
            {
                case ShipControllerType.Player:
                    controller = transform.AddComponent<ShipController>(); 
                    break;
            }

            ControllerAttached?.Invoke(controller);
        }

        private void OnMassUpdate()
        {
            rb.mass = shipData.totalMass;
            rb.inertia = shipData.totalMass;

            //HACK: Reassigning children position after updating CoM isnt great but it works for now
            Vector3 centerOfMassDelta = (Vector3)shipData.centerOfMass - centerOfMass.position;
            if (centerOfMassDelta.sqrMagnitude > 0)
            {
                centerOfMass.position = shipData.centerOfMass;
                foreach (Transform child in centerOfMass)
                    child.localPosition -= centerOfMassDelta;
            }
        }
    }
}
