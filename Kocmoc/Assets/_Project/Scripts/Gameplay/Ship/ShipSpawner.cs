using Unity.VisualScripting;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    public static class ShipSpawner
    {
        const int SHIP_GRID_SIZE = 31;

        public static Ship SpawnShip(ShipCellData[] cells, Vector2 position)
        {
            Ship ship = GameObject.Instantiate(Assets.Instance.shipPrefab, position, Quaternion.identity);
            
            ShipData shipData = (ShipData)ScriptableObject.CreateInstance(typeof(ShipData));
            shipData.Init(CreateShipGrid(cells, shipData));
            ship.Init(shipData);
            ship.AddComponent<PlayerShipController>();

            return ship;
        }

        private static Grid<ShipCellData> CreateShipGrid(ShipCellData[] cells, ShipData ship)
        {
            Grid<ShipCellData> shipGrid = new Grid<ShipCellData>(Vector2Int.one * SHIP_GRID_SIZE, centered: true);
            foreach (ShipCellData cell in cells)
            {
                cell.Init(ship);
                shipGrid.SetCell(cell.coordinates, cell);
            }

            return shipGrid;
        }
    }
}
