using System;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    [RequireComponent(typeof(Ship))]
    public class ShipController : MonoBehaviour
    {
        public Action<Vector2> ThrustDirectionUpdated;
        public Action<Transform> AttackTargetChanged;

        [HideInInspector] public ShipThrustState thrustState = ShipThrustState.Idle;
        [HideInInspector] public ShipRotationState rotationState = ShipRotationState.Idle;

        protected Ship ship;
        protected Rigidbody2D shipRb;
        protected Transform centerOfMass;

        private Transform target;

        protected Vector2 currentThrust;
        private Vector2 targetPosition;

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
            thrustState = ShipThrustState.MovingTowardsTarget;
        }

        protected void SetRotationTarget(Vector2 target)
        {
            targetAngle = CalculateAngleToTarget(target);
            rotationState = ShipRotationState.RotatingTowardsTarget;
            Debug.Log("NIGGERs");
        }

        protected void LockOnTarget(Transform target)
        {
            if (!target) UnlockRotation();
            this.target = target;
            rotationState = ShipRotationState.LockedOnTarget;
        }

        protected void UnlockRotation()
        {
            if (!target) return;
            target = null;
            rotationState = ShipRotationState.Braking;
        }

        private float CalculateAngleToTarget(Vector2 targetPosition)
        {
            Vector2 directionToTarget = (targetPosition - (Vector2)centerOfMass.position).normalized;
            return Mathf.Atan2(directionToTarget.x, directionToTarget.y) * Mathf.Rad2Deg;
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

            float brakingAngle = (Mathf.Pow(shipRb.angularVelocity, 2)) / (2f * ship.data.angularAcceleration / shipRb.inertia) * Time.deltaTime;

            if (Mathf.Abs(angleDelta) > brakingAngle)
                ApplyTorque((int)Mathf.Sign(angleDelta));
            else 
                DecreaseAngularVelocity();
        }

        private void DecreaseAngularVelocity()
        {
            if (Mathf.Abs(shipRb.angularVelocity) > 0)
                ApplyTorque((int)Mathf.Sign(shipRb.angularVelocity));
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
                case ShipRotationState.LockedOnTarget:
                    targetAngle = CalculateAngleToTarget(target.position);
                    RotateTowardsTarget(); break;
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
            LockedOnTarget,
        }
    }
}
