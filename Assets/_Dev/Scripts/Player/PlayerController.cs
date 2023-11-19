using UnityEngine;

namespace Picker.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speedModifier;

        [SerializeField] private float verticalSpeed;
        [SerializeField] private float sensitivity;

        private Rigidbody _rb;
        private float _horizontalSpeed;

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
            new Vector3(Mathf.Clamp(_horizontalSpeed, -10, 10), _rb.velocity.y, verticalSpeed + speedModifier);

        private void MovementInput()
        {
            if (Input.GetMouseButton(0))
            {
                _horizontalSpeed = Input.GetAxis("Mouse X") * sensitivity;
            }
            else _horizontalSpeed = 0;
        }
    }
}