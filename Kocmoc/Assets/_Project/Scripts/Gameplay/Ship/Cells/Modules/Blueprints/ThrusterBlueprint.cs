using Unity.VisualScripting;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    [CreateAssetMenu(fileName = "ThrusterBlueprint", menuName = "Ship/Modules/Thruster Blueprint")]
    public class ThrusterBlueprint : ModuleBlueprint
    {
        public float thrustForce;
        public override ModuleData CreateDataClass() => new ThrusterData(this);
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

        public override Module CreateModuleComponent(ShipCell cell)
        {
            Module module = cell.AddComponent<Thruster>();
            module.Init(cell, this);
            return module;
        }
        public ThrusterData(ThrusterBlueprint blueprint) : base(blueprint) { }
    }
}
