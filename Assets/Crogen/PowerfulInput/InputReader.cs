using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Crogen.PowerfulInput
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Crogen/InputReader", order = 0)]
    public class InputReader : ScriptableObject, Controls.IPlayerActions
    {
        #region Input Event
        public event Action<bool> OnLeftMouseClickEvent;
        public event Action<Vector3> OnMouseMoveEvent;
        public Vector3 MousePosition { get; private set; }
        #endregion

        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        public void OnLeftMouseClick(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnLeftMouseClickEvent?.Invoke(true);
            else if (context.canceled)
                OnLeftMouseClickEvent?.Invoke(false);
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
            MousePosition = new Vector3(MousePosition.x, MousePosition.y, -Camera.main.transform.position.z);
            OnMouseMoveEvent?.Invoke(MousePosition);
        }
    }
}