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

            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput   = Input.GetAxisRaw("Vertical");

            if (horizontalInput != 0 || verticalInput != 0) // Manual steering
            {
                movingTowardsTarget = false;
                Vector2 thrustDirection = (centerOfMass.right * horizontalInput + centerOfMass.up * verticalInput).normalized;
                SetThrustDirection(thrustDirection);
                return;
            }

            if (Input.GetKey(KeyCode.Space)) // Manual brake
            {
                movingTowardsTarget = false;
                SetThrustDirection(Vector2.ClampMagnitude(-shipRb.linearVelocity, 1));
                return;
            }

            if (Input.GetMouseButtonDown(1)) // Autopilot
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SetRotationTarget(mousePos);

                if (!Input.GetKey(KeyCode.LeftShift))
                    SetPositionTarget(mousePos);
            }

            if (!movingTowardsTarget && currentThrust.sqrMagnitude > 0) // No input
                SetThrustDirection(Vector2.zero);
        }
    }
}
