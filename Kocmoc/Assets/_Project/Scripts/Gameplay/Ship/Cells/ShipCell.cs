using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class ShipCell : MonoBehaviour
    {
        public Ship ship { get; private set; }
        public ShipCellData data { get; private set; }

        private Module[] modules;

        public Module GetModule(ModuleType type)
        {
            foreach (Module module in modules)
                if (module.type == type) return module;
            return null;
        }

        public void Init(Ship ship, ShipCellData data)
        {
            this.data = data;
            this.ship = ship;

            modules = GetComponents<Module>();
            foreach (Module module in modules) module.Init();
            
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, data.currentRotation.ToAngle()));
        }
    }
}
