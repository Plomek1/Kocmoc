using System;
using UnityEngine;
using Kocmoc.Gameplay;
using DG.Tweening;

namespace Kocmoc.UI
{
    public class ShipEditor : MonoBehaviour
    {
        public Action Opened;
        public Action Closed;

        public bool active {  get; private set; }

        [SerializeField] private Ship ship;
        
        private Grid<ShipCellData> grid => ship.data.grid;
        private ShipController shipController;
        private GridRenderer shipGridRenderer;
        [Space(10)]

        [SerializeField] private GridSelector gridSelector;
        private CameraMovement cameraMovement;

        private ShipCellBlueprint selectedBlueprint;
        private Rotation currentRotation = Rotation.Up;

        private void Awake()
        {
            cameraMovement = Camera.main.GetComponent<CameraMovement>();
            ConnectCallbacks();

            if (ship) SetShip(ship);
        }

        private void Update()
        {
            if (!active) return;
            if (selectedBlueprint && Input.GetKeyDown(KeyCode.R)) SetRotation(currentRotation.Next(skipIndexZero: true));
        }

        public void SelectBlueprint(ShipCellBlueprint blueprint)
        {
            selectedBlueprint = blueprint;
            SetRotation(Rotation.Up);

            gridSelector.fitToGroup = false;

            gridSelector.highlightSpriteRenderer.sprite = selectedBlueprint.icon;
            gridSelector.highlightSpriteRenderer.size = blueprint.size;
            
            gridSelector.DeselectCell();
            gridSelector.SetHighlightCellValidation(true);
        }

        public void DeselectBlueprint()
        {
            selectedBlueprint = null;
            SetRotation(Rotation.Up);

            gridSelector.fitToGroup = true;
            
            gridSelector.ResetHighlightSprite();
            gridSelector.SetHighlightCellValidation(false);
        }

        public void SetShip(Ship ship)
        {
            this.ship = ship;
            shipController = ship.GetComponent<ShipController>();
            shipGridRenderer = ship.gridRenderer;
            gridSelector.grid = grid;
        }

        private void SetRotation(Rotation rotation)
        {
            if (currentRotation == rotation) return;
            currentRotation = rotation;

            Vector3 targetRendererRotation = grid.origin.rotation.eulerAngles + new Vector3(0, 0, currentRotation.Angle());
            gridSelector.highlightSpriteRenderer.transform.DOLocalRotate(targetRendererRotation, .04f).SetEase(Ease.OutCubic);
            gridSelector.UpdateHighlightCell();
        }

        private void OnCellSelected(Vector2Int selectedCell, Vector2Int? previousSelectedCell)
        {
            if (selectedBlueprint)
            {
                ShipCellData cellData = new ShipCellData(selectedBlueprint, selectedCell, currentRotation);
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
            active = true;
            shipController.enabled = false;
            SetRotation(Rotation.Up);

            shipGridRenderer.Activate();
            gridSelector.Activate();

            cameraMovement.ResetPosition();
            Opened?.Invoke();
        }

        public void Close()
        {
            active = false;
            shipController.enabled = true;
            SetRotation(Rotation.Up);

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
            if (selectedBlueprint.size == Vector2Int.one) return grid.IsOccupied(coordinates) == false;

            Vector2Int rotatedSize = selectedBlueprint.size.RightAngleRotate(currentRotation);
            
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
