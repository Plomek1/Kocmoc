using System;
using UnityEngine;

namespace Kocmoc
{
    public class GridSelector : MonoBehaviour
    {
        public Action<Vector2Int> CellHighlighted;
        public Action<Vector2Int> CellSelected;
        public Action<Vector2Int> CellDeselected;

        public bool active { get; private set; }

        [SerializeField] private SpriteRenderer highlightSprite;
        [SerializeField] private SpriteRenderer selectSprite;

        private Vector2Int highlightedCell;
        private Vector2Int selectedCell;
        
        private GridBase grid;
        
        private void Update()
        {
            if (!active || grid == null) return;
            
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int hoveredCell = grid.PositionToCell(mousePos);

            if (highlightedCell != hoveredCell) HighlightCell(hoveredCell);
        }

        private void HighlightCell(Vector2Int cell)
        {
            highlightedCell = cell;
            Debug.Log(highlightedCell);
        }

        private void SelectCell(Vector2Int cell)
        {

        }

        private void DeselectCell()
        {

        }

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
            active = false;
        }

        public void SetGrid(GridBase grid) => this.grid = grid;

        public GridBase GetGrid() => this.grid;
    }
}
