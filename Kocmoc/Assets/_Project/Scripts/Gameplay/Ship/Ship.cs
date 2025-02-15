using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class Ship : MonoBehaviour
    {
        public ShipData shipData;

        private void Start()
        {
            
        }

        public void Init(ShipData shipData)
        {
            this.shipData = shipData;
        }
    }
}
