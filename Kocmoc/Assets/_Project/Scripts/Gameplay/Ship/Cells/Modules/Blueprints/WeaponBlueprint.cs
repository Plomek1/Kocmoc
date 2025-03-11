using UnityEngine;

namespace Kocmoc.Gameplay
{
    [CreateAssetMenu(fileName = "WeaponBlueprint", menuName = "Ship/Modules/Weapon Blueprint")]
    public class WeaponBlueprint : ModuleBlueprint
    {
        [field: Header("Shooting")]
        [field: SerializeField] public Projectile projectilePrefab { get; private set; }

        [field: SerializeField] public DamageData damage { get; private set; }
        
        [field: Space(5)]
        [field: SerializeField] public float projectileSpeed { get; private set; }
        [field: SerializeField] public float projectileLifetime { get; private set; }
        [field: SerializeField] public float shootCooldown { get; private set; }

        [field: Header("Barrel rotation")]
        [field: Range(0f, .999f)]
        [field: SerializeField] public float shootOffset { get; private set; }
        [field: SerializeField] public float rotationSpeed { get; private set; }
        [field: SerializeField] public float rotationRange { get; private set; }

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
