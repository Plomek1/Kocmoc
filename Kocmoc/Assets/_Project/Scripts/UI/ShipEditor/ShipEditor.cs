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
            shipGridRenderer.StartRendering();
            cameraDrag.ResetPosition();
        }

        public override void Close()
        {
            base.Close();
            shipController.enabled = true;
            shipGridRenderer.StopRendering();
        }

        
    }
}
