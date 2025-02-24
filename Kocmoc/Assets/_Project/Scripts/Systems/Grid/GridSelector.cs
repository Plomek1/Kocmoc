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

        private Sprite defaultHighlightSprite;
        private Sprite defaultSelectSprite;

        private Vector2Int? highlightedCell;
        private Vector2Int? selectedCell;
        
        private GridBase grid;

        private void Start()
        {
            defaultHighlightSprite = highlightSprite.sprite;
            //defaultSelectSprite = selectSprite.sprite;
        }

        private void Update()
        {
            if (!active || grid == null) return;
            
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int hoveredCell = grid.PositionToCell(mousePos);

            if (grid.ValidateInput(hoveredCell))
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
            CellHighlighted?.Invoke(highlightedCell.Value);
        }

        private void SelectCell(Vector2Int? cell)
        {

        }

        private void DeselectCell()
        {

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
    }
}
