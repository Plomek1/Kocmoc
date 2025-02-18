using System;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    public abstract class ModuleBlueprint : ScriptableObject
    {
        public ModuleType type;
        public abstract ModuleData CreateDataClass(ShipCellData cell);
    }

    public abstract class ModuleData
    {
        protected ModuleBlueprint blueprint;
        public ModuleType type => blueprint.type;

        public ShipCellData cell {  get; protected set; }

        public virtual void Init() { if (cell.ship) OnShipAttach(); }
        public virtual void OnShipAttach() { }
        public virtual void OnShipDettach() { }

        public abstract Module CreateModuleComponent(ShipCell cell);
        protected ModuleData(ModuleBlueprint blueprint, ShipCellData cell) 
        { 
            this.blueprint = blueprint; 
            this.cell = cell;
        }
    }

    [Flags]
    public enum ModuleType
    {
        None = 0,
        ControlRoom =    1 << 0,
        PowerGenerator = 1 << 1,
        PowerStorage =   1 << 2,
        Thruster =       1 << 3,
    }
}
