using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kocmoc
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Settings/Input Reader", order = 0)]
    public class InputReader : ScriptableObject, InputSystemActions.IShipActions, InputSystemActions.ICameraActions, InputSystemActions.IUIActions
    {
        public Action ShipSetDestination;
        public Action ShipRotate;

        public Action<Vector2> ShipManualThrust;
        public Action<bool>    ShipManualBrake;
        public Action<int>     ShipManualRotate;

        public Action<bool>  CameraDrag;
        public Action<float> CameraZoom;
        public Action        CameraReset;

        public Action UIOpenBuildingMenu;

        public List<Action> UIBackCallbacks { get; private set; }

        private InputSystemActions inputActions;

        public void EnablePlayerActions()
        {
            if (inputActions == null)
            {
                inputActions = new InputSystemActions();
                inputActions.Ship.SetCallbacks(this);
                inputActions.Camera.SetCallbacks(this);
                inputActions.UI.SetCallbacks(this);
            }
            inputActions.Enable();
        }

        public void DisablePlayerActions()
        {
            inputActions?.Disable();
        }

        public void ResetCallbacks()
        {
            ShipSetDestination = null;
            ShipRotate = null;
            ShipManualThrust = null;
            ShipManualBrake = null;
            ShipManualRotate = null;

            CameraDrag = null;
            CameraZoom = null;
            CameraReset = null;

            UIOpenBuildingMenu = null;
            UIBackCallbacks = new List<Action>();
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

        #region User Interface
        public void OnNavigate(InputAction.CallbackContext context)
        {
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
        }

        public void OnClick(InputAction.CallbackContext context)
        {
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
        }

        public void OnBack(InputAction.CallbackContext context)
        {
            if (context.performed && UIBackCallbacks.Count > 0)
            {
                Debug.Log(UIBackCallbacks.Count);
                UIBackCallbacks[UIBackCallbacks.Count - 1].Invoke();
            }
        }

        public void OnBuildingMenu(InputAction.CallbackContext context)
        {
            if (context.performed)
                UIOpenBuildingMenu?.Invoke();
        }
        #endregion
    }
}
