using System;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    public abstract class Module : MonoBehaviour
    {
        public ModuleType type => data.type;
        public Ship ship => cell.ship;
        public ShipCell cell { get; protected set; }
        public ModuleData data { get; protected set; }

        protected ShipController controller;

        private void Start() => OnStart();

        protected virtual void OnStart()
        {
            if (ship.TryGetComponent(out ShipController controller))
                SetController(controller);
            else
                ship.ControllerAttached += SetController;
        }

        protected virtual void SetController(ShipController controller) => this.controller = controller;
        public abstract void Init();
    }
}
