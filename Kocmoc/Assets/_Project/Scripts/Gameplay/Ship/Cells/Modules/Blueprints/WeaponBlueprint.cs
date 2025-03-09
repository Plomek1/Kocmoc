using UnityEngine;

namespace Kocmoc.Gameplay
{
    [CreateAssetMenu(fileName = "WeaponBlueprint", menuName = "Ship/Modules/Weapon Blueprint")]
    public class WeaponBlueprint : ModuleBlueprint
    {
        [Header("Shooting")]
        public Projectile projectilePrefab;

        public DamageData damage;
        public float projectileSpeed;
        public float projectileLifetime;
        public float shootCooldown;

        [Header("Barrel rotation")]
        [Range(0f, .999f)]
        public float shootOffset;
        public float rotationSpeed;
        public float rotationRange;

        public override ModuleData CreateDataClass(ShipCellData cell) => new WeaponData(this, cell);
    }

    public class WeaponData : ModuleData
    {
        private new WeaponBlueprint blueprint
        {
            get => (WeaponBlueprint)base.blueprint;
            set => base.blueprint = value;
        }

        public Projectile projectilePrefab => blueprint.projectilePrefab;

        public DamageData damage => blueprint.damage;
        public float projectileSpeed => blueprint.projectileSpeed;
        public float projectileLifetime => blueprint.projectileLifetime;
        public float shootCooldown => blueprint.shootCooldown;

        public float shootOffset => blueprint.shootOffset;
        public float rotationSpeed => blueprint.rotationSpeed;
        public float rotationRange => blueprint.rotationRange;

        public WeaponData(WeaponBlueprint blueprint, ShipCellData cell) : base(blueprint, cell) { }

    }
}
