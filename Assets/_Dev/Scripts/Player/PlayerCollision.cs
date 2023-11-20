using System;
using System.Collections;
using Picker.Enums;
using Picker.Managers;
using Picker.Trigger;
using UnityEngine;

namespace Picker.Player
{
    public class PlayerCollision : MonoBehaviour
    {
        public event Action OnLevelEnd;
        public event Action OnLevelProgress;
        public event Action OnRampEnter;
        public event Action OnThrow;
        
        [SerializeField] private GameObject forceCollider;

        private GameManager _gameManager;
        private Rigidbody _rb;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _rb = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball") && other.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(Vector3.forward * 15);
            }

            if (other.CompareTag("DropOff"))
            {
                DropOffTrigger trigger = other.GetComponentInParent<DropOffTrigger>();
                other.enabled = false;

                StartCoroutine(WaitDropOff(trigger, 4));
            }

            if (other.CompareTag("RampEnter"))
            {
                _gameManager.ChangeGameState(GameState.Ramp);
                other.enabled = false;
                OnRampEnter?.Invoke();
                //TODO Next Level Create
            }

            if (other.CompareTag("Throw"))
            {
                OnThrow?.Invoke();
                _gameManager.ChangeGameState(GameState.Wait);
                _rb.AddForce(Vector3.down * 5, ForceMode.VelocityChange);
            }
        }

        private IEnumerator WaitDropOff(DropOffTrigger dropOff, float delay)
        {
            _gameManager.ChangeGameState(GameState.Wait);
            _rb.isKinematic = true;
            forceCollider.SetActive(true);

            yield return new WaitForSeconds(delay);

            forceCollider.SetActive(false);

            if (dropOff.isFilled)
            {
                OnLevelProgress?.Invoke();
                _rb.isKinematic = false;
                _gameManager.ChangeGameState(GameState.Game);
            }
            else
            {
                OnLevelEnd?.Invoke();
            }
        }
    }
}
