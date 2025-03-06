using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Kocmoc
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Settings/Input Reader", order = 0)]
    public class InputReader : ScriptableObject, InputSystemActions.IShipActions, InputSystemActions.ICameraActions
    {
        public Action ShipSetDestination;
        public Action ShipRotate;

        public Action<Vector2> ShipManualThrust;
        public Action<bool>    ShipManualBrake;
        public Action<int>     ShipManualRotate;

        public Action<bool>  CameraDrag;
        public Action<float> CameraZoom;
        public Action        CameraReset;

        private InputSystemActions inputActions;

        public void EnablePlayerActions()
        {
            if (inputActions == null)
            {
                inputActions = new InputSystemActions();
                inputActions.Ship.SetCallbacks(this);
                inputActions.Camera.SetCallbacks(this);
            }
            inputActions.Enable();
        }

        public void DisablePlayerActions()
        {
            inputActions?.Disable();
        }

        #region Ship
        public void OnSetDestination(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (Keyboard.current.shiftKey.isPressed)
                    ShipRotate?.Invoke();
                else
                    ShipSetDestination?.Invoke();
            }
        }

        public void OnSetThrust(InputAction.CallbackContext context)
        {
            if (context.performed || context.canceled)
                ShipManualThrust?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnBrake(InputAction.CallbackContext context)
        {
            if (context.performed)
                ShipManualBrake?.Invoke(true);
            else if (context.canceled)
                ShipManualBrake?.Invoke(false);
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            if (context.performed || context.canceled)
                ShipManualRotate?.Invoke(context.ReadValue<int>());
        }
        #endregion


        #region Camera
        public void OnDrag(InputAction.CallbackContext context)
        {
            if (context.performed)
                CameraDrag?.Invoke(true);
            if (context.canceled)
                CameraDrag?.Invoke(false);
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            if (context.performed && Utility.IsMouseOverUI() == false)
                CameraZoom?.Invoke(context.ReadValue<float>());
        }

        public void OnReset(InputAction.CallbackContext context)
        {
            if (context.performed)
                CameraReset?.Invoke();
        }
        #endregion
    }
}
