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
            if (selectedBlueprint)
            {
                if (Input.GetKeyDown(KeyCode.R))
                    SetRotation(currentRotation.Next(skipIndexZero: true));
            }
            else
            {
                if (gridSelector.selectedCell.HasValue && Input.GetKeyDown(KeyCode.Backspace))
                {
                    if (gridSelector.selectedCell.Value == Vector2Int.zero)
                    {
                        Debug.Log("Cant remove control cell");
                        return;
                    }

                    ship.RemoveCell(gridSelector.selectedCell.Value);
                    gridSelector.DeselectCell();
                }
            }
        }

        public void SelectBlueprint(ShipCellBlueprint blueprint)
        {
            selectedBlueprint = blueprint;
            SetRotation(Rotation.Up);

            gridSelector.fitToGroup = false;

            gridSelector.highlightSpriteRenderer.sprite = selectedBlueprint.icon;
            gridSelector.highlightSpriteRenderer.size = blueprint.size;
            
            gridSelector.DeselectCell();
            gridSelector.SetHighlightValidation(true);
            gridSelector.SetSelectValidation(false);
        }

        public void DeselectBlueprint()
        {
            selectedBlueprint = null;
            SetRotation(Rotation.Up);

            gridSelector.fitToGroup = true;
            
            gridSelector.ResetHighlightSprite();
            gridSelector.SetHighlightValidation(false);
            gridSelector.SetSelectValidation(true);
        }

        public void SetShip(Ship ship)
        {
            this.ship = ship;
            shipController = ship.GetComponent<ShipController>();
            shipGridRenderer = ship.gridRenderer;
            gridSelector.grid = ship.data.grid;
        }

        private void SetRotation(Rotation rotation)
        {
            if (currentRotation == rotation) return;
            currentRotation = rotation;

            Vector3 targetRendererRotation = ship.data.grid.origin.rotation.eulerAngles + new Vector3(0, 0, currentRotation.ToAngle());
            gridSelector.highlightSpriteRenderer.transform.DOLocalRotate(targetRendererRotation, .04f).SetEase(Ease.OutCubic);
            gridSelector.UpdateHighlightSelector();
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
            gridSelector.ValidateHighlightCellFunc = ValidateCellPlacement;
            gridSelector.ValidateSelectCellFunc = ValidateCellSelection;
        }

        private bool ValidateCellPlacement(Vector2Int coordinates)
        {
            if (selectedBlueprint.size == Vector2Int.one) return ship.data.grid.IsOccupied(coordinates) == false;

            Vector2Int rotatedSize = selectedBlueprint.size.RightAngleRotate(currentRotation);
            
            for (int y = 0; y < Mathf.Abs(rotatedSize.y); y++)
            {
                for (int x = 0; x < Mathf.Abs(rotatedSize.x); x++)
                {
                    Vector2Int currentCoordinates = coordinates + new Vector2Int(x * (int)Mathf.Sign(rotatedSize.x), y * (int)Mathf.Sign(rotatedSize.y));
                    if (ship.data.grid.ValidateInput(currentCoordinates) == false || ship.data.grid.IsOccupied(currentCoordinates)) return false;
                }
            }

            return true;
        }

        private bool ValidateCellSelection(Vector2Int coordinates)
        {
            return ship.data.grid.IsOccupied(coordinates);
        }
    }
}
