using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Menu editorMenu;

        [SerializeField] private ShipBlueprint starterShip;

        private Ship playerShip;

        void Start()
        {
            Application.targetFrameRate = 144;
            playerShip = ShipSpawner.SpawnShip(starterShip, Vector2.zero);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
                editorMenu.Toggle();

            if (Input.GetKeyDown(KeyCode.Escape)) 
                Application.Quit();
        }

        private void OnDisable()
        {
            ShipSpawner.ClearCallbacks();
        }
    }
}
