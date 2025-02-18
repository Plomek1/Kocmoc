using UnityEngine;

namespace Kocmoc.Gameplay
{
    [RequireComponent(typeof(Ship))]
    public class ShipController : MonoBehaviour
    {
        private const float ROTATION_ANGLE_TRESHOLD = 1f;

        private Ship ship;

        private Vector2 rotationTarget;
        private bool rotatingTowardsTarget;

        private Transform centerOfMass => ship.GetCenterOfMass();

        private void Start()
        {
            ship = GetComponent<Ship>();
        }

        protected void SetRotationTarget(Vector2 target)
        {
            float angleToTarget = Vector2.Angle(centerOfMass.up, target - (Vector2)centerOfMass.localPosition);
            if (angleToTarget < ROTATION_ANGLE_TRESHOLD) return;
            Debug.Log(angleToTarget);
            rotationTarget = target - (Vector2)centerOfMass.localPosition;
            rotationTarget.Normalize();

            float crossProduct = Vector3.Cross(centerOfMass.up, rotationTarget.normalized).z;
            ship.rotationSpeedTarget = 10000 * ship.shipData.rotationAcceleration * Mathf.Sign(crossProduct);

            rotatingTowardsTarget = true;
        }

        private void HandleRotation()
        {
            if (!rotatingTowardsTarget) return;

            float brakingAngle = (ship.currentRotationSpeed * ship.currentRotationSpeed) / (2f * ship.shipData.rotationAcceleration);
            float angleToTarget = Vector2.Angle(centerOfMass.up, rotationTarget.normalized);
            if (brakingAngle >= angleToTarget)
            {
                ship.rotationSpeedTarget = 0;
                rotatingTowardsTarget = false;
                return;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (rotatingTowardsTarget)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(centerOfMass.position, centerOfMass.position + transform.up * 100);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(centerOfMass.position, centerOfMass.position + (Vector3)rotationTarget);
            }
        }

        private void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            HandleRotation();
        }
    }
}
