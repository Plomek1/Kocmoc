using System;
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

        private bool movingTowardsTarget;

        private Rotation activeThrustersRotation;
        private Vector2 targetPosition;
        private Vector2 targetLinearVelocity;

        private bool rotatingTowardsTarget;

        private float targetAngle;
        private float targetAngularVelocity;

        private const float MOVEMENT_STOP_TRESHOLD = .1f;
        private const float ROTATION_STOP_TRESHOLD = .001f;

        protected void SetPositionTarget(Vector2 target)
        {
            targetPosition = target;
            movingTowardsTarget = true;
        }

        protected void SetRotationTarget(Vector2 target)
        {
            Vector2 directionToTarget = (target - (Vector2)centerOfMass.position).normalized;
            targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.y) * Mathf.Rad2Deg;
            activeThrustersRotation = ship.shipData.GetStrongestThrustDirection();

            rotatingTowardsTarget = true;
        }

        private void HandleMovement()
        {
            if (!movingTowardsTarget) return;

            Vector2 targetPositionDelta = targetPosition - (Vector2)centerOfMass.position;
            float timeToStop = shipRb.linearVelocity.magnitude / ship.shipData.thrustForces[Rotation.Down];

            Vector2 desiredVelocity = timeToStop > 0 ? targetPositionDelta / timeToStop: targetPositionDelta;
            
            
            Vector2 accelerationNeeded = (desiredVelocity - shipRb.linearVelocity) * Time.deltaTime;
            des = accelerationNeeded;

            float verticalAcceleration   = Vector2.Dot(accelerationNeeded, centerOfMass.up);
            float horizontalAcceleration = Vector2.Dot(accelerationNeeded, centerOfMass.right);

            Debug.Log(accelerationNeeded);
            if (verticalAcceleration > 0) verticalAcceleration = Mathf.Min(verticalAcceleration, ship.shipData.thrustForces[Rotation.Up]);
            else verticalAcceleration = Mathf.Max(-verticalAcceleration, ship.shipData.thrustForces[Rotation.Down]);

            horizontalAcceleration = Mathf.Clamp(horizontalAcceleration, -ship.shipData.thrustForces[Rotation.Left], ship.shipData.thrustForces[Rotation.Right]);

            Vector2 thrustDirection = ((verticalAcceleration * centerOfMass.up) + (horizontalAcceleration * centerOfMass.right)).normalized;
            if (targetPositionDelta.magnitude > MOVEMENT_STOP_TRESHOLD)
            {
                ThrustDirectionUpdated?.Invoke(thrustDirection);
                currentThrust = thrustDirection;
            }
            else
            {
                movingTowardsTarget = false;
                shipRb.linearVelocity = Vector2.zero;

                ThrustDirectionUpdated?.Invoke(Vector2.zero);
                currentThrust = Vector2.zero;
            }
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

        private Vector2 currentThrust;
        private Vector2 des;
        private void OnDrawGizmos()
        {
            if (!movingTowardsTarget) return;
            Vector2 directionToTarget = targetPosition - (Vector2)centerOfMass.position;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(centerOfMass.position, (Vector2)centerOfMass.position + directionToTarget);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(centerOfMass.position, (Vector2)centerOfMass.position + des);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(centerOfMass.position, (Vector2)centerOfMass.position + currentThrust);
        }
    }

    public enum ShipControllerType
    {
        Player,
        AI
    }
}
