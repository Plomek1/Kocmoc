using UnityEngine;

namespace Kocmoc.Gameplay
{
    public abstract class Module : MonoBehaviour
    {
        public ModuleType type => data.type;

        public Ship ship => cell.ship;
        public ShipCell cell {  get; private set; }
        public ModuleData data {  get; protected set; }

        public virtual void Init(ShipCell cell, ModuleData data)
        {
            this.cell = cell;
            this.data = data;
        }
    }
}
