using UnityEngine;

namespace Picker.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speedModifier;

        [SerializeField] private float speed, turnSensitivity;

        private Rigidbody _rb;
        private float _turnSpeed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
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

        private void VelocityControl() => _rb.velocity =
            new Vector3(Mathf.Clamp(_turnSpeed, -10, 10), _rb.velocity.y, speed + speedModifier);

        private void MovementInput()
        {
            if (Input.GetMouseButton(0))
            {
                _turnSpeed = Input.GetAxis("Mouse X") * turnSensitivity;
            }
            else _turnSpeed = 0;
        }
    }
}