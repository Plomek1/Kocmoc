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
            Assets.Instance.inputReader.EnablePlayerActions();
            Assets.Instance.inputReader.UIBackCallbacks.Push(() => { Application.Quit(); Debug.Log("QUITTING..."); });


            SetPlayerShip(ShipSpawner.SpawnShip(starterShip, Vector2.zero));
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
