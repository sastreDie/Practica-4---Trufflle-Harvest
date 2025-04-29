using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInputActions;

namespace Platformer
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "GameQ/InputReader")]
    public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions 
    {
        public event UnityAction<Vector2> Move = delegate {};
        public event UnityAction<Vector2, bool> Look = delegate {};
        public event UnityAction EnableMouseControlCamera = delegate {};
        public event UnityAction DisableMouseControlCamera = delegate {};
        public event UnityAction<bool> Jump = delegate {};
        public event UnityAction<bool> Fire = delegate {};
        
        PlayerInputActions inputActions;

        public Vector3 Direction => (Vector3)inputActions.Player.Move.ReadValue<Vector2>();

        void OnEnable() {
            if (inputActions == null) {
                inputActions = new PlayerInputActions();
                inputActions.Player.SetCallbacks(this);
            }
        }
        
        public void EnablePlayerActions() {
            inputActions.Enable();
        }
        
        public void DisablePlayerActions() 
        {
            inputActions.Disable();
        }


         public void OnMove(InputAction.CallbackContext context){
            Move.Invoke(context.ReadValue<Vector2>());
         }
      
        public void OnLook(InputAction.CallbackContext context){
            Look.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
         }

        private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

        public void OnFire(InputAction.CallbackContext context){
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Fire.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Fire.Invoke(false);
                    break;
            }
         }
        public void OnMouseControlCamera(InputAction.CallbackContext context){
            switch(context.phase)
            {
                case InputActionPhase.Started:
                    EnableMouseControlCamera.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    DisableMouseControlCamera.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
         }
        public void OnRun(InputAction.CallbackContext context){
            //noop
         }
        public void OnJump(InputAction.CallbackContext context){
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Jump.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Jump.Invoke(false);
                    break;
            }
        }
    }
}
