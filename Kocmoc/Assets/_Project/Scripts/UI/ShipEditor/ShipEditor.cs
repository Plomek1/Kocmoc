using UnityEngine;
using Kocmoc.Gameplay;

namespace Kocmoc.UI
{
    public class ShipEditor : Menu
    {
        [SerializeField]private Ship ship;
        private ShipController shipController;

        private CameraMovement cameraMovement;
        private GridSelector gridSelector;
        private GridRenderer shipGridRenderer;

        private void Awake()
        {
            ShipSpawner.shipSpawned += OnShipSpawned;
            cameraMovement = Camera.main.GetComponent<CameraMovement>();
            gridSelector = GetComponent<GridSelector>();

            if (ship) SetShip(ship);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (ship.inMotion)
                {
                    Debug.Log("Can't open ship editor, stop first!");
                    return;
                }
                Toggle();
            }
        }

        public void SetShip(Ship ship)
        {
            this.ship = ship;
            shipController = ship.GetComponent<ShipController>();
            shipGridRenderer = ship.gridRenderer;
            gridSelector.SetGrid(ship.data.grid);
        }

        private void OnShipSpawned(Ship ship)
        {
            if (ship.type == ShipType.Player)
                SetShip(ship);
        }

        public override void Open()
        {
            base.Open();
            shipController.enabled = false;
            shipGridRenderer.Activate();
            gridSelector.Activate();

            cameraMovement.ResetPosition();
        }

        public override void Close()
        {
            base.Close();
            shipController.enabled = true;
            shipGridRenderer.Deactivate();
            gridSelector.Deactivate();

        }


    }
}
