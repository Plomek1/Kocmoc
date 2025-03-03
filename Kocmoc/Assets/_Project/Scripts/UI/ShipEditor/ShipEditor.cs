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

        private Ship ship;

        private ShipController shipController;
        private GridRenderer shipGridRenderer;
        private GridSelector shipGridSelector;

        [Space(10)]

        private CameraMovement cameraMovement;

        private ShipCellData selectedCell;

        private void Awake()
        {
            cameraMovement = Camera.main.GetComponent<CameraMovement>();
            ShipSpawner.shipSpawned += OnShipSpawned;
            selectedCell = null;

            if (ship) SetShip(ship);
        }

        private void Update()
        {
            if (!active) return;
            if (selectedCell != null)
            {
                if (Input.GetKeyDown(KeyCode.R))
                    SetRotation(FindNextPossibleRotation(!Input.GetKey(KeyCode.LeftShift)));
            }
            else
            {
                if (shipGridSelector.selectedCell.HasValue && Input.GetKeyDown(KeyCode.Backspace))
                {
                    Vector2Int selectedCell = shipGridSelector.selectedCell.Value;


                    if (selectedCell == Vector2Int.zero)
                    {
                        Debug.Log("Cant remove control cell");
                        return;
                    }

                    if (ship.data.grid.IsInGroup(selectedCell, out GridGroup group)) selectedCell = group.origin;

                    if (ship.data.GetDanglingCells(ship.data.grid.CoordinatesToIndex(selectedCell)).Count > 0)
                    {
                        Debug.Log("Cant remove as it would leave dangling cell ");
                        return;
                    }

                    ship.RemoveCell(selectedCell);
                    shipGridSelector.DeselectCell();
                }
            }
        }

        public void SelectBlueprint(ShipCellBlueprint blueprint)
        {
            if (selectedCell?.blueprint == blueprint) return;
            selectedCell = new ShipCellData(blueprint, Vector2Int.zero, Rotation.Up);

            SetRotation(Rotation.Up);

            shipGridSelector.fitToGroup = false;

            shipGridSelector.highlightSpriteRenderer.sprite = selectedCell.icon;
            shipGridSelector.highlightSpriteRenderer.size = blueprint.size;
            
            shipGridSelector.DeselectCell();
            shipGridSelector.SetHighlightValidation(true);
            shipGridSelector.SetSelectValidation(false);
        }

        public void DeselectBlueprint()
        {
            if (selectedCell == null) return;
            selectedCell = null;
            shipGridSelector.fitToGroup = true;
            shipGridSelector.SetHighlightValidation(false);
            shipGridSelector.SetSelectValidation(true);
            shipGridSelector.ResetHighlightSprite();
        }

        private Rotation FindNextPossibleRotation(bool clockwise = true)
        {
            Rotation rotation = selectedCell.currentRotation;
            while (true)
            {
                rotation = clockwise ? rotation.Next(true) : rotation.Previous(true);
                if (selectedCell.validRotations.HasFlag(rotation))
                    return rotation;
            }
        }

        private void SetRotation(Rotation rotation)
        {
            if (selectedCell.currentRotation != rotation)
                selectedCell.Rotate(rotation);
            
            Vector3 targetRendererRotation = new Vector3(0, 0, rotation.ToAngle());
            shipGridSelector.highlightSpriteRenderer.transform.DOLocalRotate(targetRendererRotation, .04f).SetEase(Ease.OutCubic);
            shipGridSelector.UpdateHighlightSelector();
        }

        private void OnCellSelected(Vector2Int selectedCellCoordinates, Vector2Int? previousSelectedCellCoordinates)
        {
            if (selectedCell != null)
            {
                ship.AddCell(selectedCell);
                shipGridSelector.DeselectCell();
                selectedCell = new ShipCellData(selectedCell.blueprint, selectedCell.coordinates, selectedCell.currentRotation);
            }
        }

        private void OnCellHighlighted(Vector2Int highlightedCell)
        {
            if (selectedCell == null) return;
            selectedCell.Move(highlightedCell);
        }

        private void OnShipSpawned(Ship ship)
        {
            if (ship.type == ShipType.Player)
                SetShip(ship);
        }

        public void SetShip(Ship ship)
        {
            if (this.ship)
                ClearCallbacks();

            this.ship = ship;
            shipController = ship.GetComponent<ShipController>();
            shipGridRenderer = ship.gridRenderer;
            shipGridSelector = ship.gridSelector;

            SetCallbacks();
        }

        private void SetCallbacks()
        {
            shipGridSelector.CellHighlighted += OnCellHighlighted;
            shipGridSelector.CellSelected += OnCellSelected;
            shipGridSelector.ValidateHighlightCellFunc = ValidateCellPlacement;
            shipGridSelector.ValidateSelectCellFunc = ValidateCellSelection;
        }

        private void ClearCallbacks()
        {
            shipGridSelector.SetHighlightValidation(false);
            shipGridSelector.SetSelectValidation(false);
            shipGridSelector.CellHighlighted -= OnCellHighlighted;
            shipGridSelector.CellSelected -= OnCellSelected;
            shipGridSelector.ValidateSelectCellFunc = null;
            shipGridSelector.ValidateHighlightCellFunc = null;
        }

        public void Open()
        {
            active = true;
            shipController.enabled = false;
            shipGridSelector.occupiedOnly = false;
            
            shipGridRenderer.Activate();
            cameraMovement.ResetPosition();
            
            Opened?.Invoke();
        }

        public void Close()
        {
            active = false;
            shipController.enabled = true;
            shipGridSelector.occupiedOnly = true;
            
            shipGridRenderer.Deactivate();
            
            Closed?.Invoke();
        }

        private bool ValidateCellPlacement(Vector2Int coordinates)
        {
            return ship.data.CanPlaceCell(selectedCell);
        }

        private bool ValidateCellSelection(Vector2Int coordinates)
        {
            return ship.data.grid.IsOccupied(coordinates);
        }
    }
}
