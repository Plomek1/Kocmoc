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

        public GridBase grid;
        public bool active { get; private set; }

        [Header("Renderers")]
        [field: SerializeField] public SpriteRenderer highlightSpriteRenderer { get; private set; }
        [field: SerializeField] public SpriteRenderer selectSpriteRenderer { get; private set; }

        private Sprite defaultHighlightSprite;
        private Sprite defaultSelectSprite;

        [Header("Cell Validation")]
        [SerializeField] private bool validateHighlightCell;
        [SerializeField] private Color validColor;
        [SerializeField] private Color invalidColor;
        private Color defaultColor;

        private bool highlightCellValid;
        public Func<Vector2Int, bool> ValidateInputFunc;

        [Header("Group Handling")]
        public bool fitToGroup;

        private Vector2Int? highlightedCell;
        private Vector2Int? selectedCell;
        
        private void Start()
        {
            defaultHighlightSprite = highlightSpriteRenderer.sprite;
            defaultSelectSprite = selectSpriteRenderer.sprite;
            defaultColor = highlightSpriteRenderer.color;

            if (!validateHighlightCell) highlightCellValid = true;
            grid.GridUpdated += ValidateHighlightCell;
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

            if (Input.GetMouseButtonDown(0))
            {
                if (cellValid)
                {
                    if (selectedCell.HasValue && selectedCell.Value == hoveredCell)
                        DeselectCell();
                    else
                        if (highlightCellValid) SelectCell(hoveredCell);
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
                highlightSpriteRenderer.gameObject.SetActive(false);
                return;
            }
            
            UpdateSelectorTransform(highlightSpriteRenderer, highlightedCell.Value);
            ValidateHighlightCell();
            CellHighlighted?.Invoke(highlightedCell.Value);
        }

        private void SelectCell(Vector2Int cell)
        {
            Vector2Int? previousSelectedCell = selectedCell;
            selectedCell = cell;

            UpdateSelectorTransform(selectSpriteRenderer, selectedCell.Value);

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

        private void UpdateSelectorTransform(SpriteRenderer selector, Vector2Int coordinates)
        {
            bool centerCellPosition = selector.sprite.pivot != Vector2.zero;
            Vector2 targetPosition;
            Vector2 targetSize;

            if (fitToGroup && grid.IsInGroup(coordinates, out GridGroup group))
            {
                targetPosition = grid.GetCellWorldPosition(group.origin, centerCellPosition);
                targetSize = group.size;
            }
            else
            {
                targetPosition = grid.GetCellWorldPosition(coordinates, centerOfCell: centerCellPosition);
                targetSize = Vector2.one;
            }

            if (selector.gameObject.activeSelf == false)
            {
                selector.gameObject.SetActive(true);
                selector.transform.position = targetPosition;
                selector.size = targetSize;
            }
            else
            {
                selector.transform.DOMove(targetPosition, .04f).SetEase(Ease.OutCubic);
                DOTween.To(() => highlightSpriteRenderer.size, x => selector.size = x, targetSize, .05f);
            }

            selector.transform.rotation = grid.origin.rotation;
        }


        #region Cell validation
        public void SetHighlightCellValidation(bool validate)
        {
            validateHighlightCell = validate;
            if (!validateHighlightCell)
            {
                highlightCellValid = true;
                highlightSpriteRenderer.color = defaultColor;
            }
            else ValidateHighlightCell();
        }

        private void ValidateHighlightCell()
        {
            if (validateHighlightCell && highlightedCell.HasValue)
            {
                highlightCellValid = ValidateInputFunc != null ? ValidateInputFunc(highlightedCell.Value) : true;
                highlightSpriteRenderer.color = highlightCellValid ? validColor : invalidColor;
            }
        }
        #endregion

        public void ResetHighlightSprite() => highlightSpriteRenderer.sprite = defaultHighlightSprite;
        public void ResetSelectSprite() => selectSpriteRenderer.sprite = defaultSelectSprite;

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
