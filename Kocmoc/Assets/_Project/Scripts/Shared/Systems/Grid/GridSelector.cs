using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kocmoc
{
    public class GridSelector : MonoBehaviour
    {
        public Action<Vector2Int> CellHighlighted;
        public Action<Vector2Int, Vector2Int?> CellSelected;
        public Action<Vector2Int> CellDeselected;

        public GridBase grid {  get; private set; }
        [field: SerializeField] public bool active { get; private set; }

        public Vector2Int? highlightedCell {  get; private set; }
        public Vector2Int? selectedCell    {  get; private set; }

        [field: Space(10), Header("Renderers")]
        [field: SerializeField] public SpriteRenderer highlightSpriteRenderer { get; private set; }
        [field: SerializeField] public SpriteRenderer selectSpriteRenderer { get; private set; }

        private Sprite defaultHighlightSprite;
        private Sprite defaultSelectSprite;

        [Header("Cell Validation")]
        [SerializeField] private bool validateHighlightCell;
        [SerializeField] private bool validateSelectCell;
        [SerializeField] private Color validColor;
        [SerializeField] private Color invalidColor;
        private Color defaultColor;

        public Func<Vector2Int, bool> ValidateHighlightCellFunc;
        public Func<Vector2Int, bool> ValidateSelectCellFunc;
        
        private bool highlightCellValid;

        [Header("Other")]
        public bool occupiedOnly;
        public bool fitToGroup;

        private void Start()
        {
            defaultHighlightSprite = highlightSpriteRenderer.sprite;
            defaultSelectSprite = selectSpriteRenderer.sprite;
            defaultColor = highlightSpriteRenderer.color;

            if (!validateHighlightCell) highlightCellValid = true;
        }

        private void Update()
        {
            if (!active || grid == null) return;

            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (highlightedCell.HasValue) HighlightCell(null);
                return;
            }

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int hoveredCell = grid.PositionToCell(mousePos);

            bool cellValid = grid.ValidateInput(hoveredCell);

            if (occupiedOnly && grid.IsOccupied(hoveredCell) == false)
                cellValid = false;

            if (Input.GetMouseButtonDown(0))
            {
                if (cellValid)
                {
                    if (selectedCell.HasValue && selectedCell.Value == hoveredCell)
                        DeselectCell();
                    else if (highlightCellValid)
                        SelectCell(hoveredCell);
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
            if (highlightedCell.HasValue) CellHighlighted?.Invoke(highlightedCell.Value);
            
            UpdateHighlightSelector();
        }

        private void SelectCell(Vector2Int cell)
        {
            if (validateSelectCell && ValidateSelectCellFunc(cell) == false)
            {
                DeselectCell();
                return;
            }

            Vector2Int? previousSelectedCell = selectedCell;
            selectedCell = cell;
            UpdateSelectorTransform(selectSpriteRenderer, selectedCell.Value, tween: false);
            CellSelected?.Invoke(selectedCell.Value, previousSelectedCell);
        }

        public void DeselectCell()
        {
            if (!selectedCell.HasValue) return;

            Vector2Int lastSelectedCell = selectedCell.Value;
            selectedCell = null;

            selectSpriteRenderer.gameObject.SetActive(false);
            CellDeselected?.Invoke(lastSelectedCell);
        }

        #region Selector handling
        public void UpdateHighlightSelector()
        {
            if (highlightedCell.HasValue == false)
            {
                highlightSpriteRenderer.gameObject.SetActive(false);
                return;
            }
            if (validateHighlightCell && highlightedCell.HasValue)
            {
                highlightCellValid = ValidateHighlightCellFunc != null ? ValidateHighlightCellFunc(highlightedCell.Value) : true;
                highlightSpriteRenderer.color = highlightCellValid ? validColor : invalidColor;
            }

            UpdateSelectorTransform(highlightSpriteRenderer, highlightedCell.Value);
        }

        private void UpdateSelectorTransform(SpriteRenderer selector, Vector2Int coordinates, bool tween = true)
        {
            Vector2 targetPosition;
            Vector2 targetSize;

            if (fitToGroup)
            {
                if (grid.IsInGroup(coordinates, out GridGroup group))
                {
                    targetPosition = grid.GetGroupCenterPosition(group);
                    targetSize = group.sizeAbs;
                }
                else
                {
                    targetPosition = grid.GetCellPosition(coordinates, centerOfCell: true);
                    targetSize = Vector2.one;
                }
            }
            else
            {
                targetPosition = grid.GetCellPosition(coordinates, centerOfCell: true);
                targetSize = selector.size;
            }

            if (selector.gameObject.activeSelf == false || !tween)
            {
                selector.gameObject.SetActive(true);
                selector.transform.localPosition = targetPosition;
                selector.size = targetSize;
            }
            else
            {
                selector.transform.DOLocalMove(targetPosition, .04f).SetEase(Ease.OutCubic);
                DOTween.To(() => highlightSpriteRenderer.size, x => selector.size = x, targetSize, .04f).SetEase(Ease.OutCubic);
            }
        }

        public void SetHighlightValidation(bool validate)
        {
            validateHighlightCell = validate;
            if (!validateHighlightCell)
            {
                highlightCellValid = true;
                highlightSpriteRenderer.color = defaultColor;
            }
            UpdateHighlightSelector();
        }

        public void SetSelectValidation(bool validate)
        {
            validateSelectCell = validate;
        }

        public void ResetHighlightSprite() => highlightSpriteRenderer.sprite = defaultHighlightSprite;
        public void ResetSelectSprite() => selectSpriteRenderer.sprite = defaultSelectSprite;
        #endregion

        public void SetGrid(GridBase grid)
        {
            if (this.grid != null)
                this.grid.GridUpdated -= UpdateHighlightSelector;

            this.grid = grid;

            if (this.grid.origin)
            {
                highlightSpriteRenderer.transform.SetParent(grid.origin);
                selectSpriteRenderer.transform.SetParent(grid.origin);
            }

            this.grid.GridUpdated += UpdateHighlightSelector;
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
            HighlightCell(null);
            DeselectCell();
            active = false;
        }
    }
}
