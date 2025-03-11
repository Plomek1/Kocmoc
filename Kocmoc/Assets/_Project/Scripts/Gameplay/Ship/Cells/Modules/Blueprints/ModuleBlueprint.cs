using UnityEngine;

namespace Kocmoc.Gameplay
{
    public abstract class ModuleBlueprint : ScriptableObject
    {
        [field: SerializeField] public ModuleType type { get; private set; }
        public abstract ModuleData CreateDataClass(ShipCellData cell);
    }

    public abstract class ModuleData
    {
        protected ModuleBlueprint blueprint;
        public ModuleType type => blueprint.type;

        public ShipCellData cell {  get; protected set; }

        public virtual void Init() { if (cell.ship != null) OnShipAttach(); }
        protected virtual void OnShipAttach() { }
        protected virtual void OnShipDettach() { }

        protected ModuleData(ModuleBlueprint blueprint, ShipCellData cell)
        { 
            this.blueprint = blueprint; 
            this.cell = cell;
        }
    }

    public enum ModuleType
    {
        ControlRoom,
        PowerGenerator,
        PowerStorage,
        Thruster,
        Weapon,
    }
}
