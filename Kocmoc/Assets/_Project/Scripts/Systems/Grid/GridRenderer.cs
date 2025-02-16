using com.cyborgAssets.inspectorButtonPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Kocmoc
{
    public class GridRenderer : MonoBehaviour
    {
        public bool rendering { get; private set; }

        [SerializeField] private RenderType drawType;
        [Space(20)]

        [Header("Sprite Rendering")]
        [SerializeField] private Transform spritesRoot;
        [SerializeField] private Sprite cellSprite;
        [Space(10)]

        [Header("Line rendering")]
        [SerializeField] private float lineWidth = 1;
        [SerializeField] private Color lineColor;
        [Space(20)]

        private GridBase gridToDraw = null;
        private List<GameObject> cellSprites;
        private SpriteRenderer repeatingSprite;
        private LineRenderer lineRenderer;

        private bool initialized;

        void Start()
        {
            if (initialized) return;
            Init();
        }

        void Update()
        {
            if (!rendering || gridToDraw == null) return;
            lineRenderer.startWidth = Camera.main.orthographicSize * 0.001f * lineWidth;
        }

        private void Init()
        {
            if (initialized) return;

            lineRenderer = gameObject.GetComponent<LineRenderer>();
            lineRenderer.material.color = lineColor;
            cellSprites = new List<GameObject>();
            initialized = true;
        }

        public void SetGrid(GridBase grid)
        {
            if (grid == null) return;
            if (!initialized) Init();
            gridToDraw = grid;

            switch (drawType)
            {
                case RenderType.Lines:
                    UpdateLineRenderer();
                    break;
                case RenderType.Sprites:
                    UpdateSprites();
                    break;
                case RenderType.SpritesRepeating:
                    UpdateRepeatingSprite();
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
                case RenderType.Lines:
                    lineRenderer.enabled = true;
                    break;
                case RenderType.Sprites:
                    spritesRoot.gameObject.SetActive(true);
                    break;
                case RenderType.SpritesRepeating:
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
                case RenderType.Lines:
                    lineRenderer.enabled = false;
                    break;
                case RenderType.Sprites:
                    spritesRoot.gameObject.SetActive(false);
                    break;
                case RenderType.SpritesRepeating:
                    spritesRoot.gameObject.SetActive(false);
                    break;
            }

            rendering = false;
        }

        private void UpdateSprites()
        {
            int cellCount = gridToDraw.cellCount;

            for (int i = 0; i < gridToDraw.cellCount; i++)
            {
                int cellIndex = gridToDraw.centered ? gridToDraw.CenterInput(i) : i;
                Vector2 spritePos = gridToDraw.GetCellPosition(cellIndex, true);

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

        private void UpdateRepeatingSprite()
        {
            if (!repeatingSprite)
            {
                repeatingSprite = spritesRoot.AddComponent<SpriteRenderer>();
                repeatingSprite.sprite = cellSprite;
                repeatingSprite.drawMode = SpriteDrawMode.Tiled;
            }
            Vector2 gridCenter = gridToDraw.centered ? Vector2.zero: gridToDraw.worldSize * .5f;
            repeatingSprite.transform.localPosition = gridCenter;
            repeatingSprite.size = gridToDraw.size;
        }

        private void UpdateLineRenderer()
        {
            List<Vector3> positions = new List<Vector3>();

            float gridWidth  = gridToDraw.size.x * gridToDraw.cellSize;
            float gridHeight = gridToDraw.size.y * gridToDraw.cellSize;

            Vector2 nextPos = (Vector2)transform.position;
            if (gridToDraw.centered) nextPos -= gridToDraw.worldSize * .5f;
            positions.Add(nextPos);

            int polarity = 1;
            for (int x = 0; x <= gridToDraw.size.x; x++)
            {
                nextPos += new Vector2(0, gridHeight) * polarity;
                positions.Add(nextPos);

                if (x < gridToDraw.size.x)
                {
                    nextPos += new Vector2(gridToDraw.cellSize, 0);
                    positions.Add(nextPos);
                }

                polarity *= -1;
            }

            int verticalDirection = polarity;
            polarity = -1;

            for (int y = 0; y <= gridToDraw.size.y; y++)
            {
                nextPos += new Vector2(gridWidth, 0) * polarity;
                positions.Add(nextPos);

                if (y < gridToDraw.size.y)
                {
                    nextPos += new Vector2(0, gridToDraw.cellSize * verticalDirection);
                    positions.Add(nextPos);
                }

                polarity *= -1;
            }

            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }
        
        public enum RenderType
        {
            Lines,
            Sprites,
            SpritesRepeating
        }
    }
}
