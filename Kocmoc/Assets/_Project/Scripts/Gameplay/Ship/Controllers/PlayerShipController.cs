using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class PlayerShipController : ShipController
    {
        [SerializeField] private LayerMask shipCellDetectionLayers;

        protected override void OnStart()
        {
            base.OnStart();
            Camera.main.GetComponent<CameraMovement>().target = centerOfMass;

            Globals.Instance.inputReader.ShipCommand      += ExecuteCommand;
            Globals.Instance.inputReader.ShipManualThrust += ManualThrust;
            Globals.Instance.inputReader.ShipManualBrake  += ManualBrake;
            Globals.Instance.inputReader.ShipManualRotate += ManualRotation;
        }

        private void ExecuteCommand(bool move, bool rotate)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, shipCellDetectionLayers);
            if (hit.collider && hit.collider.TryGetComponent(out ShipCell cell))
            {
                if (cell.ship != ship)
                {
                    AttackTargetChanged?.Invoke(cell.transform);
                    LockOnTarget(cell.transform);
                    return;
                }
            }
            if (move)   SetPositionTarget(mousePos);
            if (rotate) SetRotationTarget(mousePos);
        }

        private void ManualThrust(Vector2 direction)
        {
            thrustState = ShipThrustState.ManualThrust;

            Vector2 thrustDirection = Quaternion.AngleAxis(centerOfMass.rotation.eulerAngles.z, Vector3.forward) * direction;
            SetThrustDirection(thrustDirection);
        }

        private void ManualBrake(bool activate)
        {
            if (activate)
            {
                thrustState = ShipThrustState.Braking;
            }
            else
            {
                thrustState = ShipThrustState.Idle;
                SetThrustDirection(Vector2.zero);
            }
        }

        private void ManualRotation(float direction)
        {
            if (direction == 0)
            {
                rotationState = ShipRotationState.Braking;
                return;
            }
            rotationState = ShipRotationState.ManualRotation;
        }
    }
}
