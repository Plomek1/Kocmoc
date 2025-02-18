using System;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    public abstract class ModuleBlueprint : ScriptableObject
    {
        public ModuleType type;
        public abstract ModuleData CreateDataClass();
    }

    public abstract class ModuleData
    {
        protected ModuleBlueprint blueprint;
        public ModuleType type => blueprint.type;

        public abstract Module CreateModuleComponent(ShipCell cell);
        protected ModuleData(ModuleBlueprint blueprint) => this.blueprint = blueprint;
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
