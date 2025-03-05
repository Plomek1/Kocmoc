using PlasticGui.WorkspaceWindow.QueryViews.Changesets;
using System;
using UnityEditor;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    [RequireComponent(typeof(Ship))]
    public class ShipController : MonoBehaviour
    {
        public Action<Vector2> ThrustDirectionUpdated;

        [HideInInspector] public ShipThrustState thrustState = ShipThrustState.Idle;
        [HideInInspector] public ShipRotationState rotationState = ShipRotationState.Idle;

        protected Ship ship;
        protected Rigidbody2D shipRb;
        protected Transform centerOfMass;

        protected Vector2 currentThrust;

        private Vector2 targetPosition;

        private float targetAngle;
        private float targetAngularVelocity;

        private const float MOVEMENT_STOP_TRESHOLD = .1f;
        private const float ROTATION_STOP_TRESHOLD = .001f;

        protected void SetThrustDirection(Vector2 direction)
        {
            Debug.Log(direction);

            currentThrust = direction;
            ThrustDirectionUpdated?.Invoke(currentThrust);
        }

        protected void SetPositionTarget(Vector2 target)
        {
            targetPosition = target;
            thrustState = ShipThrustState.MovingTowardsTarget;
        }

        protected void SetRotationTarget(Vector2 target)
        {
            Vector2 directionToTarget = (target - (Vector2)centerOfMass.position).normalized;
            targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.y) * Mathf.Rad2Deg;

            rotationState = ShipRotationState.RotatingTowardsTarget;
        }

        private void MoveTowardsTarget()
        {
            Vector2 targetPositionDelta = targetPosition - (Vector2)centerOfMass.position;

            //TODO calculate stopping distance
            if (targetPositionDelta.magnitude <= MOVEMENT_STOP_TRESHOLD && shipRb.linearVelocity.magnitude <= MOVEMENT_STOP_TRESHOLD)
            {
                SetThrustDirection(Vector2.zero);
                shipRb.linearVelocity = Vector2.zero;
                thrustState = ShipThrustState.Idle;
                return;
            }

            float thrustDirectionX = targetPositionDelta.x - shipRb.linearVelocity.x;
            float thrustDirectionY = targetPositionDelta.y - shipRb.linearVelocity.y;
            Vector2 thrustDirection = Vector2.ClampMagnitude(new Vector2(thrustDirectionX, thrustDirectionY), 1);
            SetThrustDirection(thrustDirection);
        }

        private void DecreaseLinearVelocity()
        {
            SetThrustDirection(Vector2.ClampMagnitude(-shipRb.linearVelocity, 1));
            
            if (shipRb.linearVelocity.magnitude <= MOVEMENT_STOP_TRESHOLD)
            {
                SetThrustDirection(Vector2.zero);
                shipRb.linearVelocity = Vector2.zero;
                thrustState = ShipThrustState.Idle;
            }
        }

        private void RotateTowardsTarget()
        {
            float currentAngle = Mathf.Atan2(centerOfMass.up.x, centerOfMass.up.y) * Mathf.Rad2Deg;
            float angleDelta = Mathf.DeltaAngle(currentAngle, targetAngle);

            //TODO braking angle is too high
            float brakingAngle = (Mathf.Pow(shipRb.angularVelocity, 2)) / (2f * ship.data.angularAcceleration / shipRb.inertia) * Time.deltaTime;

            if (Mathf.Abs(angleDelta) > brakingAngle)
                ApplyTorque((int)Mathf.Sign(angleDelta));
            else rotationState = ShipRotationState.Braking;
        }

        private void DecreaseAngularVelocity()
        {
            ApplyTorque((int)Mathf.Sign(shipRb.angularVelocity));

            if (Mathf.Abs(shipRb.angularVelocity) == 0)
            {
                shipRb.angularVelocity = 0;
                rotationState = ShipRotationState.Idle;
            }
        }

        private void ApplyTorque(int direction)
        {
            float torgue = -direction * ship.data.angularAcceleration;
            shipRb.AddTorque(torgue);
        }

        private void Start() => OnStart();
        private void FixedUpdate() => OnFixedUpdate();

        protected virtual void OnFixedUpdate()
        {
            switch(thrustState)
            {
                case ShipThrustState.MovingTowardsTarget:
                    MoveTowardsTarget(); break;
                case ShipThrustState.Braking:
                    DecreaseLinearVelocity(); break;
                default: break;
            }

            switch(rotationState)
            {
                case ShipRotationState.RotatingTowardsTarget:
                    RotateTowardsTarget(); break;
                case ShipRotationState.Braking:
                    DecreaseAngularVelocity(); break;
                default: break;
            }
        }

        protected virtual void OnStart()
        {
            ship = GetComponent<Ship>();
            shipRb = GetComponent<Rigidbody2D>();
            centerOfMass = ship.GetCenterOfMass();
        }

        public enum ShipThrustState
        {
            Idle,
            MovingTowardsTarget,
            ManualThrust,
            Braking,
        }

        public enum ShipRotationState
        {
            Idle,
            RotatingTowardsTarget,
            ManualRotation,
            Braking,
        }
    }
}
