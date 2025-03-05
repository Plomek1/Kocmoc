using System;
using UnityEngine;
using UnityEngine.Events;

namespace Kocmoc.Gameplay
{
    public class LevelManager : Singleton<LevelManager>
    {
        public Action<Ship> PlayerShipSet;

        public UnityEvent EditorMenuToggle;

        [SerializeField] private ShipBlueprint starterShip;

        public Ship playerShip {  get; private set; }

        void Start()
        {
            Application.targetFrameRate = 144;
            Assets.Instance.inputReader.EnablePlayerActions();

            SetPlayerShip(ShipSpawner.SpawnShip(starterShip, Vector2.zero));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
                EditorMenuToggle.Invoke();

            if (Input.GetKeyDown(KeyCode.Escape)) 
                Application.Quit();
        }
    
        public void SetPlayerShip(Ship ship)
        {
            playerShip = ship;
            PlayerShipSet?.Invoke(playerShip);
        }

        private void OnDisable()
        {
            ShipSpawner.ClearCallbacks();
        }
    }
}
