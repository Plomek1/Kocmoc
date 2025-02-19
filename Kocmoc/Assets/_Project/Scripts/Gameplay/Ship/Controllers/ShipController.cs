using UnityEngine;

namespace Kocmoc.Gameplay
{
    [RequireComponent(typeof(Ship))]
    public class ShipController : MonoBehaviour
    {
        private const float ROTATION_STOP_TRESHOLD = .001f;

        protected Ship ship;
        protected Rigidbody2D shipRb;

        private bool rotatingTowardsTarget;
        private float targetAngle;

        private float angularVelocityTarget;

        protected void SetRotationTarget(Vector2 target)
        {
            Vector2 directionToTarget = (target - shipRb.position).normalized;
            targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.y) * Mathf.Rad2Deg;
            
            rotatingTowardsTarget = true;
        }

        private void HandleRotation()
        {
            if (!rotatingTowardsTarget) return;

            float currentAngle = Mathf.Atan2(transform.up.x, transform.up.y) * Mathf.Rad2Deg;
            float angleDelta = Mathf.DeltaAngle(currentAngle, targetAngle);
            float brakingAngle = (shipRb.angularVelocity * shipRb.angularVelocity * Time.deltaTime) / (2f * ship.shipData.angularAcceleration) * shipRb.inertia;

            if (Mathf.Abs(angleDelta) > brakingAngle) angularVelocityTarget = Mathf.Sign(angleDelta) * ship.shipData.angularAcceleration;
            else angularVelocityTarget = 0;

            if (Mathf.Abs(angleDelta) > ROTATION_STOP_TRESHOLD * ship.shipData.angularAcceleration)
            {
                float torgue;
                if (angularVelocityTarget == 0) torgue = Mathf.Sign(shipRb.angularVelocity) * ship.shipData.angularAcceleration * -1;
                else torgue = Mathf.Sign(angularVelocityTarget) * ship.shipData.angularAcceleration * -1;
                shipRb.AddTorque(torgue);
            }
            else
            {
                shipRb.rotation = -targetAngle;
                shipRb.angularVelocity = 0;
                rotatingTowardsTarget = false;
            }
        }

        private void Start() => OnStart();
        private void FixedUpdate() => OnFixedUpdate();

        protected virtual void OnFixedUpdate()
        {
            HandleRotation();
        }

        protected virtual void OnStart()
        {
            ship = GetComponent<Ship>();
            shipRb = GetComponent<Rigidbody2D>();
        }
    }

    public enum ShipControllerType
    {
        Player,
        AI
    }
}
