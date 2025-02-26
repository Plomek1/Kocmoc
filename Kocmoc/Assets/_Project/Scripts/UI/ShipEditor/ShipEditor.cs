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

        private ShipCellBlueprint selectedBlueprint;

        private void Awake()
        {
            cameraMovement = Camera.main.GetComponent<CameraMovement>();
            ConnectCallbacks();

            if (ship) SetShip(ship);
        }

        public void SelectBlueprint(ShipCellBlueprint blueprint)
        {
            selectedBlueprint = blueprint;
            gridSelector.highlightSpriteRenderer.sprite = selectedBlueprint.icon;
            gridSelector.DeselectCell();
            gridSelector.SetHighlightCellValidation(true);
            gridSelector.fitToGroup = false;
        }

        public void DeselectBlueprint()
        {
            selectedBlueprint = null;
            gridSelector.ResetHighlightSprite();
            gridSelector.SetHighlightCellValidation(false);
            gridSelector.fitToGroup = true;
        }

        public void SetShip(Ship ship)
        {
            this.ship = ship;
            shipController = ship.GetComponent<ShipController>();
            shipGridRenderer = ship.gridRenderer;
            gridSelector.grid = ship.data.grid;
        }

        private void OnCellSelected(Vector2Int selectedCell, Vector2Int? previousSelectedCell)
        {
            if (selectedBlueprint)
            {
                ShipCellData cellData = new ShipCellData(selectedBlueprint, selectedCell, Rotation.Up);
                ship.AddCell(cellData);
                gridSelector.DeselectCell();
            }
        }

        private void OnCellHighlighted(Vector2Int highlightedCell)
        {
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
            gridSelector.CellHighlighted += OnCellHighlighted;
            gridSelector.CellSelected += OnCellSelected;
            gridSelector.ValidateInputFunc = ValidateCellPosition;
        }

        private bool ValidateCellPosition(Vector2Int coordinates)
        {
            return ship.data.grid.IsOccupied(coordinates) == false;
        }
    }
}
