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
        public new ThrusterBlueprint blueprint
        {
            get => (ThrusterBlueprint)base.blueprint;
            private set => base.blueprint = value;
        }

        public float thrustForce => blueprint.thrustForce;

        public override void OnShipAttach()
        {
            cell.ship.thrustForces[cell.currentRotation] += thrustForce;
        }

        public override void OnShipDettach()
        {
            cell.ship.thrustForces[cell.currentRotation] -= thrustForce;
        }

        public override Module CreateModuleComponent(ShipCell cell)
        {
            Module module = cell.gameObject.AddComponent<Thruster>();
            module.Init(cell, this);
            return module;
        }

        public ThrusterData(ThrusterBlueprint blueprint, ShipCellData cell) : base(blueprint, cell) { }
    }
}
