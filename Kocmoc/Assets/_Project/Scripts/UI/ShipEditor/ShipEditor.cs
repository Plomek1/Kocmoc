using System;
using UnityEngine;
using Kocmoc.Gameplay;

namespace Kocmoc.UI
{
    public class ShipEditor : MonoBehaviour
    {
        public Action Opened;
        public Action Closed;

        [SerializeField]private Ship ship;
        private ShipController shipController;
        private GridRenderer shipGridRenderer;
        [Space(10)]

        [SerializeField] private GridSelector gridSelector;

        private CameraMovement cameraMovement;

        private void Awake()
        {
            cameraMovement = Camera.main.GetComponent<CameraMovement>();
            ConnectCallbacks();

            if (ship) SetShip(ship);
        }

        public void SelectCell(ShipCellBlueprint cell)
        {
            gridSelector.GetHighlightSprite().sprite = cell.icon;
        }

        public void DeselectCell()
        {
            gridSelector.ResetHighlightSprite();
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

        public void Open()
        {
            shipController.enabled = false;
            shipGridRenderer.Activate();
            gridSelector.Activate();

            cameraMovement.ResetPosition();
            Opened?.Invoke();
        }

        public void Close()
        {
            shipController.enabled = true;
            shipGridRenderer.Deactivate();
            gridSelector.Deactivate();
            Closed?.Invoke();
        }

        private void ConnectCallbacks()
        {
            ShipSpawner.shipSpawned += OnShipSpawned;

        }
    }
}
