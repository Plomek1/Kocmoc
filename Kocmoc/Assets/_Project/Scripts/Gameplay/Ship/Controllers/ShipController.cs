using System;
using UnityEditor;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    [RequireComponent(typeof(Ship))]
    public class ShipController : MonoBehaviour
    {
        public Action<Vector2> ThrustDirectionUpdated;

        protected Ship ship;
        protected Rigidbody2D shipRb;

        protected Transform centerOfMass;

        protected Vector2 currentThrust;

        protected bool movingTowardsTarget;
        private Vector2 targetPosition;

        protected bool rotatingTowardsTarget;
        private float targetAngle;
        private float targetAngularVelocity;

        private const float MOVEMENT_STOP_TRESHOLD = .1f;
        private const float ROTATION_STOP_TRESHOLD = .001f;

        protected void SetThrustDirection(Vector2 direction)
        {
            currentThrust = direction;
            ThrustDirectionUpdated?.Invoke(currentThrust);
        }

        protected void SetPositionTarget(Vector2 target)
        {
            targetPosition = target;
            movingTowardsTarget = true;
        }

        protected void SetRotationTarget(Vector2 target)
        {
            Vector2 directionToTarget = (target - (Vector2)centerOfMass.position).normalized;
            targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.y) * Mathf.Rad2Deg;

            rotatingTowardsTarget = true;
        }

        private void HandleMovement()
        {
            if (!movingTowardsTarget) return;

            Vector2 targetPositionDelta = targetPosition - (Vector2)centerOfMass.position;

            if (targetPositionDelta.magnitude <= MOVEMENT_STOP_TRESHOLD && shipRb.linearVelocity.magnitude <= MOVEMENT_STOP_TRESHOLD)
            {
                SetThrustDirection(Vector2.zero);
                shipRb.linearVelocity = Vector2.zero;
                movingTowardsTarget = false;
                return;
            }

            float thrustDirectionX = targetPositionDelta.x - shipRb.linearVelocity.x;
            float thrustDirectionY = targetPositionDelta.y - shipRb.linearVelocity.y;
            Vector2 thrustDirection = Vector2.ClampMagnitude(new Vector2(thrustDirectionX, thrustDirectionY), 1);
            SetThrustDirection(thrustDirection);
        }

        private void HandleRotation()
        {
            if (!rotatingTowardsTarget) return;

            float currentAngle = Mathf.Atan2(centerOfMass.up.x, centerOfMass.up.y) * Mathf.Rad2Deg;
            float angleDelta = Mathf.DeltaAngle(currentAngle, targetAngle);

            float brakingAngle = (Mathf.Pow(shipRb.angularVelocity, 2) * Time.deltaTime) / (2f * ship.shipData.angularAcceleration / shipRb.inertia);

            if (Mathf.Abs(angleDelta) > brakingAngle) targetAngularVelocity = Mathf.Sign(angleDelta) * ship.shipData.angularAcceleration;
            else targetAngularVelocity = 0;

            if (Mathf.Abs(angleDelta) > ROTATION_STOP_TRESHOLD * ship.shipData.angularAcceleration)
            {
                float torgue;
                if (targetAngularVelocity == 0) torgue = Mathf.Sign(shipRb.angularVelocity) * ship.shipData.angularAcceleration * -1;
                else torgue = Mathf.Sign(targetAngularVelocity) * ship.shipData.angularAcceleration * -1;
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
            HandleMovement();
            HandleRotation();
        }

        protected virtual void OnStart()
        {
            ship = GetComponent<Ship>();
            shipRb = GetComponent<Rigidbody2D>();
            centerOfMass = ship.GetCenterOfMass();
        }
    }
}
