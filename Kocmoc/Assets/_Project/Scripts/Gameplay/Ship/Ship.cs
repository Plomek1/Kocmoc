using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class Ship : MonoBehaviour
    {
        public ShipData shipData;

        [SerializeField] private Transform centerOfMass;
        [SerializeField] private Transform cellsRoot;

        private GridRenderer gridRenderer;

        public void Init(ShipData shipData)
        {
            this.shipData = shipData;
            shipData.CenterOfMassChanged += OnCenterOfMassUpdate;

            foreach (GridCell<ShipCellData> cell in shipData.grid.GetCells())
            {
                ShipCell cellGo = Instantiate(cell.data.prefab, shipData.grid.GetCellPosition(cell.coordinates), Quaternion.identity, cellsRoot);
                cellGo.Init(this, cell.data);
            }

            OnCenterOfMassUpdate();

            gridRenderer = GetComponentInChildren<GridRenderer>();
            gridRenderer.SetGrid(shipData.grid);
            gridRenderer.StartRendering();

            Camera.main.GetComponent<CameraDrag>().SetTarget(transform);
        }

        private void OnCenterOfMassUpdate()
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

        private void Update()
        {
            //transform.position += Vector3.up * Time.deltaTime * 5;
        }
    }
}
