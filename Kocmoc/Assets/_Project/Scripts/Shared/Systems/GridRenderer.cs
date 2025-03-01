using com.cyborgAssets.inspectorButtonPro;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoc
{
    public class GridRenderer : MonoBehaviour
    {
        public bool active { get; private set; }

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

        private GridBase grid = null;
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
            if (!active || grid == null) return;
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
            this.grid = grid;

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

        public GridBase GetGrid() => grid;

        [ProPlayButton]
        public void ToggleActivation()
        {
            if (active) Deactivate();
            else        Activate();
        }

        public void Activate()
        {
            if (active || grid == null) return;
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

            active = true;
        }

        public void Deactivate()
        {
            if (!active) return;

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

            active = false;
        }

        private void UpdateSprites()
        {
            int cellCount = grid.cellCount;

            for (int i = 0; i < grid.cellCount; i++)
            {
                int cellIndex = grid.centered ? grid.CenterInput(i) : i;
                Vector2 spritePos = grid.GetCellPosition(cellIndex, true);

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
                repeatingSprite = spritesRoot.gameObject.AddComponent<SpriteRenderer>();
                repeatingSprite.sprite = cellSprite;
                repeatingSprite.drawMode = SpriteDrawMode.Tiled;
            }
            Vector2 gridCenter = grid.centered ? Vector2.zero: grid.worldSize * .5f;
            repeatingSprite.transform.localPosition = gridCenter;
            repeatingSprite.size = grid.size;
        }

        private void UpdateLineRenderer()
        {
            List<Vector3> positions = new List<Vector3>();

            float gridWidth  = grid.size.x * grid.cellSize;
            float gridHeight = grid.size.y * grid.cellSize;

            Vector2 nextPos = (Vector2)transform.position;
            if (grid.centered) nextPos -= grid.worldSize * .5f;
            positions.Add(nextPos);

            int polarity = 1;
            for (int x = 0; x <= grid.size.x; x++)
            {
                nextPos += new Vector2(0, gridHeight) * polarity;
                positions.Add(nextPos);

                if (x < grid.size.x)
                {
                    nextPos += new Vector2(grid.cellSize, 0);
                    positions.Add(nextPos);
                }

                polarity *= -1;
            }

            int verticalDirection = polarity;
            polarity = -1;

            for (int y = 0; y <= grid.size.y; y++)
            {
                nextPos += new Vector2(gridWidth, 0) * polarity;
                positions.Add(nextPos);

                if (y < grid.size.y)
                {
                    nextPos += new Vector2(0, grid.cellSize * verticalDirection);
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
