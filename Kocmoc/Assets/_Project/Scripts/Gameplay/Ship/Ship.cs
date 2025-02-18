using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class Ship : MonoBehaviour
    {
        public ShipData shipData;

        public Vector2 velocity { get; set; }
        public float rotationSpeedTarget { get; set; }
        public float currentRotationSpeed { get; private set; }

        [SerializeField] private Transform centerOfMass;
        [SerializeField] private Transform cellsRoot;

        private GridRenderer gridRenderer;

        public Transform GetCenterOfMass() => centerOfMass;

        public void Init(ShipData shipData)
        {
            this.shipData = shipData;
            shipData.MassUpdated += OnMassUpdate;

            foreach (GridCell<ShipCellData> cell in shipData.grid.GetCells())
            {
                ShipCell cellGo = Instantiate(cell.data.prefab, shipData.grid.GetCellPosition(cell.coordinates), Quaternion.identity, cellsRoot);
                cellGo.Init(this, cell.data);
            }

            OnMassUpdate();

            gridRenderer = GetComponentInChildren<GridRenderer>();
            gridRenderer.SetGrid(shipData.grid);
            gridRenderer.StartRendering();

            Camera.main.GetComponent<CameraDrag>().SetTarget(transform);
        }

        private void OnMassUpdate()
        {
            //HACK: Reassigning children position after updating CoM isnt great but it works for now
            Vector3 centerOfMassDelta = (Vector3)shipData.centerOfMass - centerOfMass.position;
            if (centerOfMassDelta.sqrMagnitude > 0)
            {
                centerOfMass.position = shipData.centerOfMass;
                foreach (Transform child in centerOfMass)
                    child.localPosition -= centerOfMassDelta;
            }
        }

        private void FixedUpdate()
        {
            HandleRotation();
            HandleVelocity();
        }

        private void HandleRotation()
        {
            float rotationSpeedTargetDelta = Mathf.Abs(rotationSpeedTarget - currentRotationSpeed);
            if (rotationSpeedTargetDelta > 0)
                currentRotationSpeed += Mathf.Sign(rotationSpeedTarget - currentRotationSpeed) * Mathf.Min(rotationSpeedTargetDelta, shipData.rotationAcceleration);

            centerOfMass.rotation = Quaternion.Euler(0, 0, centerOfMass.rotation.eulerAngles.z + currentRotationSpeed);
        }

        private void HandleVelocity()
        {
            transform.position = transform.position + (Vector3)velocity;
        }
    }
}
