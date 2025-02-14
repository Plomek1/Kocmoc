using System.Collections.Generic;
using UnityEngine;

namespace Kocmoc
{
    public class GridRenderer : MonoBehaviour
    {
        public GridBase gridToDraw = null;
        private bool render = true;

        [SerializeField] private DrawType drawType;

        [Header("Line rendering")]
        [SerializeField] private Color lineColor;
        private LineRenderer lineRenderer;

        void Start()
        {
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            SetGrid(new Grid<int>(new Vector2Int(10, 10), cellSize: 1));
        }

        void Update()
        {
            if (!render || gridToDraw == null) return;

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
            }
        }

        public void StartRendering()
        {
            if (render) return;
            
            switch (drawType)
            {
                case DrawType.LINES:
                    lineRenderer.enabled = true;
                    break;
            }

            render = true;
        }

        public void StopRendering()
        {
            if (!render) return;

            switch (drawType)
            {
                case DrawType.LINES:
                    lineRenderer.enabled = true;
                    break;
            }

            render = false;
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
