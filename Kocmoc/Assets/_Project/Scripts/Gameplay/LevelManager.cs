using System;
using UnityEngine;
using UnityEngine.Events;

namespace Kocmoc.Gameplay
{
    public class LevelManager : Singleton<LevelManager>
    {
        public Action<Ship> PlayerShipSet;

        [SerializeField] private ShipBlueprint starterShip;

        public Ship playerShip { get; private set; }

        void Start()
        {
            Application.targetFrameRate = 144;
            Globals.Instance.inputReader.UIBackCallbacks.Add(() => { Application.Quit(); Debug.Log("QUITTING..."); });

            SetPlayerShip(ShipSpawner.SpawnShip(starterShip, ShipType.Player, Vector2.zero));
            ShipSpawner.SpawnShip(starterShip, ShipType.Wreck, Vector2.up * 10);
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
