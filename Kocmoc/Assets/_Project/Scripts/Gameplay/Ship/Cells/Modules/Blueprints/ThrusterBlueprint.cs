using UnityEngine;

namespace Kocmoc.Gameplay
{
    [CreateAssetMenu(fileName = "ThrusterBlueprint", menuName = "Ship/Modules/Thruster Blueprint")]
    public class ThrusterBlueprint : ModuleBlueprint
    {
        public float thrustForce;
        public override ModuleData CreateDataClass(ShipCellData cell) => new ThrusterData(this, cell);
    }

    [System.Serializable]
    public class ThrusterData : ModuleData
    {
        private new ThrusterBlueprint blueprint
        {
            get => (ThrusterBlueprint)base.blueprint;
            set => base.blueprint = value;
        }

        public float thrustForce => blueprint.thrustForce;

        protected override void OnShipAttach()
        {
            cell.ship.thrustForces[cell.currentRotation] += thrustForce;
        }

        protected override void OnShipDettach()
        {
            cell.ship.thrustForces[cell.currentRotation] -= thrustForce;
        }

        public ThrusterData(ThrusterBlueprint blueprint, ShipCellData cell) : base(blueprint, cell) { }
    }
}
