using UnityEngine;

namespace Kocmoc.Gameplay
{
    [CreateAssetMenu(fileName = "WeaponBlueprint", menuName = "Ship/Modules/Weapon Blueprint")]
    public class WeaponBlueprint : ModuleBlueprint
    {
        public float shootCooldown;
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

        public float rotationRange => blueprint.rotationRange;

        public WeaponData(WeaponBlueprint blueprint, ShipCellData cell) : base(blueprint, cell) { }

    }
}
