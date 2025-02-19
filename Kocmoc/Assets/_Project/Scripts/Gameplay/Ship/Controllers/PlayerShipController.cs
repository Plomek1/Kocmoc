using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class PlayerShipController : ShipController
    {
        private void Update()
        {
            base.OnFixedUpdate();
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SetRotationTarget(mousePos);
            }
        }
    }
}
