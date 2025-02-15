using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class Ship : MonoBehaviour
    {
        public ShipData shipData;

        public void Init(ShipData shipData)
        {
            this.shipData = shipData;

            Debug.Log(shipData.grid.cellCount);

            for (int i = shipData.grid.firstCell; i < shipData.grid.lastCell; i++)
            {
                ShipCellData cell = shipData.grid.GetCell(i);
                if (!cell) continue;
                Instantiate(cell.prefab, shipData.grid.GetCellPosition(i), Quaternion.identity, transform);
            }
        }
    }
}
