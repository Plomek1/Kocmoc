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

            for (int i = shipData.grid.firstCell; i < shipData.grid.lastCell; i++)
            {
                ShipCellData cell = shipData.grid.GetCell(i);
                if (cell == null) continue;
                Instantiate(cell.prefab, shipData.grid.GetCellPosition(i), Quaternion.identity, transform);
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
