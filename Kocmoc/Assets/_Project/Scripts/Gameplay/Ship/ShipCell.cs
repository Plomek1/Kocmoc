using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class ShipCell : MonoBehaviour
    {
        ShipCellData data;

        public void Init(ShipCellData data)
        {
            this.data = data;
            Debug.Log(data.prefab);
        }
    }
}
