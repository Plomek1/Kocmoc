using UnityEngine;
using Kocmoc.Gameplay;

namespace Kocmoc.UI
{
    public class ShipEditor : Menu
    {
        [SerializeField]private Ship ship;
        private ShipController shipController;

        private CameraDrag cameraDrag;
        private GridRenderer shipGridRenderer;

        private void Awake()
        {
            ShipSpawner.shipSpawned += OnShipSpawned;
            cameraDrag = Camera.main.GetComponent<CameraDrag>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (opened)
                {
                    shipGridRenderer.StopRendering();
                    Close();
                }
                else
                {
                    shipGridRenderer.StartRendering();
                    Open();
                }
            }
        }

        public void SetShip(Ship ship)
        {
            this.ship = ship;
            shipController = ship.GetComponent<ShipController>();
            shipGridRenderer = ship.gridRenderer;
        }

        private void OnShipSpawned(Ship ship)
        {
            if (ship.type == ShipType.Player)
                SetShip(ship);
        }
    }
}
