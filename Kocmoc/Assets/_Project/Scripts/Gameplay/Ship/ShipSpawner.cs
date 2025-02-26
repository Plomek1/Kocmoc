using System;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    public static class ShipSpawner
    {
        public static Action<Ship> shipSpawned;

        const int SHIP_GRID_SIZE = 31;

        public static Ship SpawnShip(ShipCellData[] cells, Vector2 position)
        {
            Ship ship = GameObject.Instantiate(Assets.Instance.shipPrefab, position, Quaternion.identity);
            
            ShipData shipData = (ShipData)ScriptableObject.CreateInstance(typeof(ShipData));
            shipData.Init(CreateShipGrid(cells));
            
            ship.Init(shipData, ShipType.Player);

            shipSpawned?.Invoke(ship);
            return ship;
        }

        private static Grid<ShipCellData> CreateShipGrid(ShipCellData[] cells)
        {
            Grid<ShipCellData> shipGrid = new Grid<ShipCellData>(Vector2Int.one * SHIP_GRID_SIZE, centered: true);
            foreach (ShipCellData cell in cells)
            {
                cell.Init();
                shipGrid.SetCell(cell.coordinates, cell);
            }

            return shipGrid;
        }
    }
}
