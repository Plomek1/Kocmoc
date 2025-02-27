using System;
using UnityEngine;
using Kocmoc.Gameplay;
using static UnityEngine.UI.Image;
using System.Drawing;

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
            Debug.Log(blueprint.size);
            gridSelector.highlightSpriteRenderer.size = blueprint.size;
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
            gridSelector.CellSelected += OnCellSelected;
            gridSelector.ValidateInputFunc = ValidateCellPosition;
        }

        private bool ValidateCellPosition(Vector2Int coordinates)
        {
            var grid = ship.data.grid;
            if (selectedBlueprint.size == Vector2Int.one) return grid.IsOccupied(coordinates) == false;

            Vector2Int rotatedSize = selectedBlueprint.size; // TODO adjust by current rotation

            for (int y = 0; y < Mathf.Abs(rotatedSize.y); y++)
            {
                for (int x = 0; x < Mathf.Abs(rotatedSize.x); x++)
                {
                    Vector2Int currentCoordinates = coordinates + new Vector2Int(x * (int)Mathf.Sign(rotatedSize.x), y * (int)Mathf.Sign(rotatedSize.y));
                    if (grid.ValidateInput(currentCoordinates) == false || grid.IsOccupied(currentCoordinates)) return false;
                }
            }

            return true;
        }
    }
}
