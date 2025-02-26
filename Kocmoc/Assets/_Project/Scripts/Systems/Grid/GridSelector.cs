using System;
using UnityEngine;

namespace Kocmoc
{
    public class GridSelector : MonoBehaviour
    {
        public Action<Vector2Int> CellHighlighted;
        public Action<Vector2Int, Vector2Int?> CellSelected;
        public Action<Vector2Int> CellDeselected;

        public bool active { get; private set; }

        [SerializeField] private SpriteRenderer highlightSprite;
        [SerializeField] private SpriteRenderer selectSprite;

        [Header("Cell Validation")]
        [SerializeField] private bool validateHighlightedCell;
        [SerializeField] private Color validColor;
        [SerializeField] private Color invalidColor;
        private Func<Vector2Int, bool> ValidateInput;
        private Color defaultColor;

        private Sprite defaultHighlightSprite;
        private Sprite defaultSelectSprite;

        private Vector2Int? highlightedCell;
        private Vector2Int? selectedCell;

        private bool highlightedCellValid;
        
        private GridBase grid;

        private void Start()
        {
            defaultHighlightSprite = highlightSprite.sprite;
            //defaultSelectSprite = selectSprite.sprite;
            defaultColor = highlightSprite.color;

            if (!validateHighlightedCell) highlightedCellValid = true;
            grid.GridUpdated += ValidateHighlightSprite;
        }

        private void Update()
        {
            if (!active || grid == null) return;
            
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int hoveredCell = grid.PositionToCell(mousePos);

            bool cellValid = grid.ValidateInput(hoveredCell);

            if (Input.GetMouseButtonDown(0))
            {
                if (cellValid)
                {
                    if (selectedCell.HasValue && selectedCell.Value == hoveredCell)
                        DeselectCell();
                    else
                        if (highlightedCellValid) SelectCell(hoveredCell);
                }
                else DeselectCell();
            }

            if (cellValid)
            {
                if (highlightedCell != hoveredCell)
                    HighlightCell(hoveredCell);
            }
            else if (highlightedCell != null)
                HighlightCell(null);

        }

        private void HighlightCell(Vector2Int? cell)
        {
            highlightedCell = cell;
            
            if (highlightedCell == null)
            {
                highlightSprite.gameObject.SetActive(false);
                return;
            }

            highlightSprite.gameObject.SetActive(true);
            highlightSprite.transform.position = grid.GetCellWorldPosition(highlightedCell.Value, centerOfCell: true);
            highlightSprite.transform.rotation = grid.origin.rotation;

            ValidateHighlightSprite();
            CellHighlighted?.Invoke(highlightedCell.Value);
        }

        private void SelectCell(Vector2Int? cell)
        {
            Vector2Int? previousSelectedCell = selectedCell;
            selectedCell = cell;
            CellSelected?.Invoke(selectedCell.Value, previousSelectedCell);
        }

        public void DeselectCell()
        {
            if (!selectedCell.HasValue) return;

            CellDeselected?.Invoke(selectedCell.Value);
            selectedCell = null;
        }

        public SpriteRenderer GetHighlightSprite() => highlightSprite;
        public SpriteRenderer GetSelectSprite() => selectSprite;

        public void ResetHighlightSprite() => highlightSprite.sprite = defaultHighlightSprite;
        public void ResetSelectSprite() => selectSprite.sprite = defaultSelectSprite;

        public void ToggleActivation()
        {
            if (active) Deactivate();
            else        Activate();
        }

        public void Activate()
        {
            if (active) return;
            active = true;
        }

        public void Deactivate() 
        {
            if (!active) return;
            HighlightCell(null);
            active = false;
        }

        public void SetGrid(GridBase grid) => this.grid = grid;
        public GridBase GetGrid() => this.grid;
    
        public void SetValidationFunction(Func<Vector2Int, bool> func) => ValidateInput = func;
    
        private void ValidateHighlightSprite()
        {
            if (validateHighlightedCell)
            {
                highlightedCellValid = ValidateInput(highlightedCell.Value);
                highlightSprite.color = highlightedCellValid ? validColor : invalidColor;
            }
        }
    }
}
