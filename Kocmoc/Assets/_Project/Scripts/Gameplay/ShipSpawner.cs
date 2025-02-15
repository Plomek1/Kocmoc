using UnityEngine;

namespace Kocmoc.Gameplay
{
    public static class ShipSpawner
    {
        const int SHIP_GRID_SIZE = 51;

        public static Ship SpawnShip(ShipCellData[] cells, Vector2 position)
        {

            Ship ship = GameObject.Instantiate(Assets.Instance.shipPrefab, position, Quaternion.identity);
            
            ShipData shipData = new ShipData();

            Grid<ShipCellData> shipGrid = new Grid<ShipCellData>(Vector2Int.one *  SHIP_GRID_SIZE, centered: true);
            shipData.grid = shipGrid;

            return ship;
        }
    }
}
