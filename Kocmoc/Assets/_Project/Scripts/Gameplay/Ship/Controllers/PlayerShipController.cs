using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class PlayerShipController : ShipController
    {
        private bool buildingMoveOpened;

        private GridRenderer gridRenderer;

        protected override void OnStart()
        {
            base.OnStart();
            Camera.main.GetComponent<CameraMovement>().target = centerOfMass;

            Globals.Instance.inputReader.ShipSetDestination += SetDestination;
            Globals.Instance.inputReader.ShipRotate         += Rotate;
            Globals.Instance.inputReader.ShipManualThrust   += ManualThrust;
            Globals.Instance.inputReader.ShipManualBrake    += ManualBrake;
            Globals.Instance.inputReader.ShipManualRotate   += ManualRotation;
        }

        private void SetDestination()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetPositionTarget(mousePos);
            SetRotationTarget(mousePos);
        }

        private void Rotate()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetRotationTarget(mousePos);
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

        }
    }
}
