using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private ShipCellData[] cells;

        void Start()
        {
            Application.targetFrameRate = 144;
            ShipSpawner.SpawnShip(cells, Vector2.zero);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        }
    }
}
