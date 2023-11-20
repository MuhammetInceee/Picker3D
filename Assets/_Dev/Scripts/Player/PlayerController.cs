using Picker.Enums;
using Picker.Managers;
using UnityEngine;

namespace Picker.Player
{
    public class PlayerController : MonoBehaviour
    {
        private float _speedModifier;

        [SerializeField] private float verticalSpeed;
        [SerializeField] private float sensitivity;

        private Rigidbody _rb;
        private GameManager _gameManager;
        private float _horizontalSpeed;

        private void Awake()
        {
            InitVariables();
        }

        private void FixedUpdate()
        {
            PlayerMovement();
        }

        private void PlayerMovement()
        {
            VelocityControl();
            MovementInput();
            ThrowInput();
        }

        private void VelocityControl()
        {
             if(_gameManager.gameState != GameState.Game && _gameManager.gameState != GameState.Ramp) return;
            
            _rb.velocity = new Vector3(Mathf.Clamp(_horizontalSpeed, -10, 10), _rb.velocity.y, verticalSpeed + _speedModifier);
        }

        private void MovementInput()
        {
            if(_gameManager.gameState != GameState.Game) return;
            
            if (Input.GetMouseButton(0))
                _horizontalSpeed = Input.GetAxis("Mouse X") * sensitivity;
            
            else _horizontalSpeed = 0;
        }

        private void ThrowInput()
        {
            if(_gameManager.gameState != GameState.Ramp) return;
            
            
        }

        private void InitVariables()
        {
            _rb = GetComponent<Rigidbody>();
            _gameManager = GameManager.Instance;
        }
    }
}