using System;
using Picker.Enums;
using UnityEngine;

namespace Picker.Player
{
    public class PlayerController : PlayerBase
    {
        internal float SpeedModifier;
        [SerializeField] private float verticalSpeed;
        [SerializeField] private float sensitivity;

        private void OnEnable()
        {
            playerCollision.OnReset += Reset;
        }

        private void OnDisable()
        {
            playerCollision.OnReset -= Reset;
        }

        private void FixedUpdate()
        {
            PlayerMovement();
        }

        private void PlayerMovement()
        {
            VelocityControl();
            MovementInput();
        }

        private void VelocityControl()
        {
            if (gameManager.gameState != GameState.Game && gameManager.gameState != GameState.Ramp) return;
            
            Rb.velocity = new Vector3(Mathf.Clamp(HorizontalSpeed, -10, 10), Rb.velocity.y,
                verticalSpeed + SpeedModifier);
        }

        private void MovementInput()
        {
            if (gameManager.gameState != GameState.Game) return;

            if (Input.GetMouseButton(0))
                HorizontalSpeed = Input.GetAxis("Mouse X") * sensitivity;

            else HorizontalSpeed = 0;
        }

        public override void Reset()
        {
            base.Reset();
            SpeedModifier = 0;
        }
    }
}