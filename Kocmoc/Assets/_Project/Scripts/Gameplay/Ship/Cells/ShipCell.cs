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

            int modulesCount = data.modules.Length;
            modules = new Module[modulesCount];
            for (int i = 0; i < modulesCount; i++)
                modules[i] = data.modules[i].CreateModuleComponent(this);
        
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            float rotationAngle = Mathf.Log((int)data.currentRotation, 2);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -rotationAngle * 90));
        }
    }
}
