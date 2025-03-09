using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class ShipCell : MonoBehaviour, IDamageable
    {
        public Ship ship { get; private set; }
        public ShipCellData data { get; private set; }

        public int health => data.health;

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

        public void RemoveFromShip()
        {
            ship.RemoveCell(data.coordinates);
            Destroy(gameObject);
        }

        public void Damage(DamageData damage)
        {
            data.health -= damage.damage;
            Debug.Log(data.health);
            if (data.health <= 0) Die();
        }

        public void Die()
        {
            if (data.coordinates == Vector2Int.zero)
            {
                ship.DestroyShip();
                return;
            }

            RemoveFromShip();
            foreach(int cell in ship.data.GetDanglingCells())
                ship.GetCell(cell).RemoveFromShip();
        }
    }
}
