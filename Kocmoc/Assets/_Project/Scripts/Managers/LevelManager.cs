using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Menu editorMenu;

        [SerializeField] private ShipCellData[] cells;

        private Ship playerShip;

        void Start()
        {
            Application.targetFrameRate = 144;
            playerShip = ShipSpawner.SpawnShip(cells, Vector2.zero);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                //if (playerShip.inMotion)
                //{
                //    Debug.Log("Can't open ship editor, stop first!");
                //    return;
                //}
                editorMenu.Toggle();
            }

            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        }
    }
}
