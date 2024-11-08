using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Crogen.PowerfulInput
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Crogen/InputReader", order = 0)]
	public class InputReader : ScriptableObject, Controls.IPlayerActions, Controls.IUIActions
    {
        #region Input Event

        public event Action MouseClickEvent;
        public event Action<Vector2> MouseMoveEvent;
        public event Action<Vector3> MoveEvent;
        public event Action DashEvent;
        public event Action AttackEvent;

        #endregion

        public Vector2 MousePosition { get; private set; }

        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
                _controls.UI.SetCallbacks(this);
            }
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if(context.performed)
                DashEvent?.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector3>());
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.performed)
                AttackEvent?.Invoke();
        }

		public void OnMouseClick(InputAction.CallbackContext context)
		{
			if (context.performed)
                MouseClickEvent?.Invoke();
		}

		public void OnMouseMove(InputAction.CallbackContext context)
		{
            MousePosition = context.ReadValue<Vector2>();

			if (context.performed)
                MouseMoveEvent?.Invoke(MousePosition);
		}
	}
}