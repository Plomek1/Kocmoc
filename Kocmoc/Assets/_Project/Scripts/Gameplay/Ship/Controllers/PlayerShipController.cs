using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class PlayerShipController : ShipController
    {
        protected override void OnStart()
        {
            base.OnStart();
            Camera.main.GetComponent<CameraDrag>().SetTarget(centerOfMass);
        }

        private void Update()
        {
            base.OnFixedUpdate();
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SetPositionTarget(mousePos);
                SetRotationTarget(mousePos);
            }
        }
    }
}
