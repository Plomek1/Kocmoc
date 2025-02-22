using UnityEngine;
using Kocmoc.Gameplay;
using System;

namespace Kocmoc.UI
{
    public class ShipEditor : MonoBehaviour
    {
        public Action EditorOpened;
        public Action EditorClosed;

        private Ship ship;
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
                gameObject.SetActive(!gameObject.activeSelf);
                shipGridRenderer.ToggleRendering();
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
