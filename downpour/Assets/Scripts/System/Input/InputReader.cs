
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Downpour;
using Downpour.Input;

namespace Downpour.Input {
    public class InputReader : SingletonPersistent<InputReader>, InputActions.IGameplayActions
    {
        public InputActions InputActions { get; private set; }

        // Events:
        public event Action<float, float, bool> MovementEvent;
        public event Action<bool> JumpEvent;
        public event Action<bool> SlashEvent;
        public event Action<bool> ParryEvent;
        public event Action<bool> DashEvent;

        // Initialization
        protected override void Awake() {
            base.Awake();

            if(InputActions == null) {
                InputActions = new InputActions();

                InputActions.Gameplay.SetCallbacks(this);
            }
        }

        private void OnDisable() {
            DisableAllInput();
        }

        // <summary>
        // Enables input reader for action map Gameplay.
        // </summary>
        public void EnableGameplayInput() {
            InputActions.Gameplay.Enable();
        }

        // <summary>
        // Disables input reader.
        // </summary>
        public void DisableAllInput() {
            InputActions.Gameplay.Disable();
        }

        // Handle Inputs:
        void InputActions.IGameplayActions.OnMovement(InputAction.CallbackContext context) {
            Vector2 inputVector = context.ReadValue<Vector2>();
            MovementEvent?.Invoke(inputVector.x, inputVector.y, context.phase == InputActionPhase.Canceled ? false : true);
        }

        // Handle Inputs:
        void InputActions.IGameplayActions.OnJump(InputAction.CallbackContext context) {
            JumpEvent?.Invoke(context.phase == InputActionPhase.Canceled ? false : true);
        }

        // Handle Inputs:
        void InputActions.IGameplayActions.OnSlash(InputAction.CallbackContext context) {
            SlashEvent?.Invoke(context.phase == InputActionPhase.Performed ? true : false);
        }

        // Handle Inputs:
        void InputActions.IGameplayActions.OnParry(InputAction.CallbackContext context) {
            ParryEvent?.Invoke(context.phase == InputActionPhase.Performed ? true : false);
        }

        // Handle Inputs:
        void InputActions.IGameplayActions.OnDash(InputAction.CallbackContext context) {
            DashEvent?.Invoke(context.phase == InputActionPhase.Performed ? true : false);
        }
    }
}