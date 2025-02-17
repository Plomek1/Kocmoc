using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class Ship : MonoBehaviour
    {
        public ShipData shipData;

        private GridRenderer gridRenderer;

        public void Init(ShipData shipData)
        {
            this.shipData = shipData;

            foreach (GridCell<ShipCellData> cell in shipData.grid.GetCells())
            {
                ShipCell cellGo = Instantiate(cell.data.prefab, shipData.grid.GetCellPosition(cell.coordinates), Quaternion.identity, transform);
                cellGo.Init(this, cell.data);
            }

            gridRenderer = GetComponentInChildren<GridRenderer>();
            gridRenderer.SetGrid(shipData.grid);
            gridRenderer.StartRendering();

            Camera.main.GetComponent<CameraDrag>().SetTarget(transform);
        }

        private void Update()
        {
            //transform.position += Vector3.up * Time.deltaTime * 5;
        }
    }
}
