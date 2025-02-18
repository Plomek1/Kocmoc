using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class PlayerShipController : ShipController
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SetRotationTarget(mousePos);
            }
        }
    }
}
