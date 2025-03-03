using UnityEngine;

namespace Kocmoc.Gameplay
{
    [CreateAssetMenu(fileName = "Ship", menuName = "Ship/Ship Blueprint", order = 0)]
    public class ShipBlueprint : ScriptableObject
    {
        public string shipName;
        public ShipCellData[] cells;
    }
}
