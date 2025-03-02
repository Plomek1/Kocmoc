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

        private ShipCellData selectedCell;

        private void Awake()
        {
            cameraMovement = Camera.main.GetComponent<CameraMovement>();
            ConnectCallbacks();
            selectedCell = null;

            if (ship) SetShip(ship);
        }

        private void Update()
        {
            if (!active) return;
            if (selectedCell != null)
            {
                if (Input.GetKeyDown(KeyCode.R))
                    SetRotation(FindNextPossibleRotation());
            }
            else
            {
                if (gridSelector.selectedCell.HasValue && Input.GetKeyDown(KeyCode.Backspace))
                {
                    Vector2Int selectedCell = gridSelector.selectedCell.Value;


                    if (selectedCell == Vector2Int.zero)
                    {
                        Debug.Log("Cant remove control cell");
                        return;
                    }

                    if (ship.data.grid.IsInGroup(selectedCell, out GridGroup group)) selectedCell = group.origin;

                    if (ship.data.GetDanglingCells(ship.data.grid.CoordinatesToIndex(selectedCell)).Count > 0)
                    {
                        foreach (var a in ship.data.GetDanglingCells(ship.data.grid.CoordinatesToIndex(selectedCell))) 
                            Debug.Log("UNCONNECTED: " + ship.data.grid.IndexToCoordinates(a));
                        Debug.Log("Cant remove as it would leave dangling cell ");
                        return;
                    }

                    ship.RemoveCell(selectedCell);
                    gridSelector.DeselectCell();
                }
            }
        }

        public void SelectBlueprint(ShipCellBlueprint blueprint)
        {
            if (selectedCell?.blueprint == blueprint) return;
            selectedCell = new ShipCellData(blueprint, Vector2Int.zero, Rotation.Up);

            SetRotation(Rotation.Up);

            gridSelector.fitToGroup = false;

            gridSelector.highlightSpriteRenderer.sprite = selectedCell.icon;
            gridSelector.highlightSpriteRenderer.size = blueprint.size;
            
            gridSelector.DeselectCell();
            gridSelector.SetHighlightValidation(true);
            gridSelector.SetSelectValidation(false);
        }

        public void DeselectBlueprint()
        {
            if (selectedCell == null) return;
            selectedCell = null;
            gridSelector.fitToGroup = true;
            gridSelector.SetHighlightValidation(false);
            gridSelector.SetSelectValidation(true);
            gridSelector.ResetHighlightSprite();
        }

        public void SetShip(Ship ship)
        {
            this.ship = ship;
            shipController = ship.GetComponent<ShipController>();
            shipGridRenderer = ship.gridRenderer;
            gridSelector.grid = ship.data.grid;
        }

        private Rotation FindNextPossibleRotation()
        {
            Rotation rotation = selectedCell.currentRotation;
            while (true)
            {
                rotation = rotation.Next(skipIndexZero: true);
                if (selectedCell.validRotations.HasFlag(rotation))
                    return rotation;
            }
        }

        private void SetRotation(Rotation rotation)
        {
            if (selectedCell.currentRotation != rotation)
                selectedCell.Rotate(rotation);
            
            Vector3 targetRendererRotation = new Vector3(0, 0, rotation.ToAngle());
            gridSelector.highlightSpriteRenderer.transform.DOLocalRotate(targetRendererRotation, .04f).SetEase(Ease.OutCubic);
            gridSelector.UpdateHighlightSelector();
        }

        private void OnCellSelected(Vector2Int selectedCellCoordinates, Vector2Int? previousSelectedCellCoordinates)
        {
            if (selectedCell != null)
            {
                ship.AddCell(selectedCell);
                gridSelector.DeselectCell();
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

        public void Open()
        {
            active = true;
            shipController.enabled = false;
            gridSelector.occupiedOnly = false;
            
            shipGridRenderer.Activate();
            cameraMovement.ResetPosition();
            
            Opened?.Invoke();
        }

        public void Close()
        {
            active = false;
            shipController.enabled = true;
            gridSelector.occupiedOnly = true;
            
            shipGridRenderer.Deactivate();
            
            Closed?.Invoke();
        }

        private void ConnectCallbacks()
        {
            ShipSpawner.shipSpawned += OnShipSpawned;
            gridSelector.CellHighlighted += OnCellHighlighted;
            gridSelector.CellSelected += OnCellSelected;
            gridSelector.ValidateHighlightCellFunc = ValidateCellPlacement;
            gridSelector.ValidateSelectCellFunc = ValidateCellSelection;
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
