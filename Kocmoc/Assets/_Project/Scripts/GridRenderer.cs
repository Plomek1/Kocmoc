using com.cyborgAssets.inspectorButtonPro;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoc
{
    public class GridRenderer : MonoBehaviour
    {
        [HideInInspector] public bool rendering { get; private set; }

        [SerializeField] private DrawType drawType;
        [SerializeField] private bool renderOnStart;
        [Space(20)]

        [Header("Sprite Rendering")]
        [SerializeField] private Transform spritesRoot;
        [SerializeField] private Sprite cellSprite;
        [Space(10)]

        [Header("Line rendering")]
        [SerializeField] private Color lineColor;
        [Space(20)]

        private List<GameObject> cellSprites;
        private GridBase gridToDraw = null;
        private LineRenderer lineRenderer;
        
        void Start()
        {
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            lineRenderer.material.color = lineColor;
            cellSprites = new List<GameObject>();
            SetGrid(new Grid<int>(new Vector2Int(12, 9), cellSize: 1));

            if (renderOnStart && gridToDraw != null)
                StartRendering();
        }

        void Update()
        {
            if (!rendering || gridToDraw == null) return;
            lineRenderer.startWidth = Camera.main.orthographicSize * 0.005f;
        }

        public void SetGrid(GridBase grid)
        {
            if (grid == null) return;
            gridToDraw = grid;

            switch (drawType)
            {
                case DrawType.LINES:
                    UpdateLineRenderer();
                    break;
                case DrawType.SPRITES:
                    UpdateSprites();
                    break;
            }
        }

        [ProPlayButton]
        public void ToggleRendering()
        {
            if (rendering) StopRendering();
            else           StartRendering();
        }

        public void StartRendering()
        {
            if (rendering || gridToDraw == null) return;
            switch (drawType)
            {
                case DrawType.LINES:
                    lineRenderer.enabled = true;
                    break;
                case DrawType.SPRITES:
                    spritesRoot.gameObject.SetActive(true);
                    break;
            }

            rendering = true;
        }

        public void StopRendering()
        {
            if (!rendering) return;

            switch (drawType)
            {
                case DrawType.LINES:
                    lineRenderer.enabled = false;
                    break;
                case DrawType.SPRITES:
                    spritesRoot.gameObject.SetActive(false);
                    break;
            }

            rendering = false;
        }

        private void UpdateSprites()
        {
            int cellCount = gridToDraw.cellCount;

            for (int i = 0; i < cellCount; i++)
            {
                Vector2 spritePos = gridToDraw.GetGridPosition(i, true);

                if(cellSprites.Count > i)
                    cellSprites[i].transform.position = spritePos;
                else
                {
                    GameObject sprite = new GameObject($"CellSprite{i}");
                    sprite.transform.SetParent(spritesRoot);
                    sprite.transform.localPosition = spritePos;
                    sprite.AddComponent<SpriteRenderer>().sprite = cellSprite;
                    cellSprites.Add(sprite);
                }   
            }
        }

        private void UpdateLineRenderer()
        {
            List<Vector3> positions = new List<Vector3>();

            float gridWidth  = gridToDraw.dimensions.x * gridToDraw.cellSize;
            float gridHeight = gridToDraw.dimensions.y * gridToDraw.cellSize;

            Vector2 nextPos = (Vector2)transform.position;
            positions.Add(nextPos);

            int polarity = 1;
            for (int x = 0; x <= gridToDraw.dimensions.x; x++)
            {
                nextPos += new Vector2(0, gridHeight) * polarity;
                positions.Add(nextPos);

                if (x < gridToDraw.dimensions.x)
                {
                    nextPos += new Vector2(gridToDraw.cellSize, 0);
                    positions.Add(nextPos);
                }

                polarity *= -1;
            }

            int verticalDirection = polarity;

            for (int y = 0; y <= gridToDraw.dimensions.y; y++)
            {
                nextPos += new Vector2(gridWidth, 0) * polarity;
                positions.Add(nextPos);

                if (y < gridToDraw.dimensions.y)
                {
                    nextPos += new Vector2(0, gridToDraw.cellSize * verticalDirection);
                    positions.Add(nextPos);
                }

                polarity *= -1;
            }

            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }
        
        public enum DrawType
        {
            LINES,
            SPRITES
        }
    }
}
