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

                StartCoroutine(WaitDropOff(trigger, 6));
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
                //TODO Level Progress Bar Will Be Increased
                _rb.isKinematic = false;
            }
            else
            {
                //TODO UI Manager's On Level Fail will Invoked
            }
        }
    }
}
