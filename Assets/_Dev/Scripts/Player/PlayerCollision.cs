using System;
using System.Collections;
using Picker.Enums;
using Picker.Helpers;
using Picker.Trigger;
using TMPro;
using UnityEngine;

namespace Picker.Player
{
    public class PlayerCollision : PlayerBase
    {
        public event Action<bool> OnLevelEnd;
        public event Action<int> OnMoneyChange; 
        public event Action OnLevelProgress;
        public event Action OnRampEnter;
        public event Action OnThrow;
        
        [SerializeField] private GameObject forceCollider;
        [SerializeField] private GameObject extraColliders;

        private Ramp _currentRamp;
        private int _endMoney;
        

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
                _currentRamp = other.GetComponent<Ramp>();
                gameManager.ChangeGameState(GameState.Ramp);
                OnRampEnter?.Invoke();
                other.enabled = false;
                SetRbConstraints(RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ);
                extraColliders.SetActive(false);
                
                HorizontalSpeed = 0;
                //TODO Next Level Create
            }

            if (other.CompareTag("Throw"))
            {
                OnThrow?.Invoke();
                gameManager.ChangeGameState(GameState.Wait);
                Rb.AddForce(Vector3.down * 2.5f, ForceMode.VelocityChange);
            }

            if (other.CompareTag("MultipleTile"))
            {
                _endMoney = int.Parse(other.GetComponentInChildren<TextMeshProUGUI>().text);
                StopCoroutine(nameof(WaitMovementEnd));
                StartCoroutine(nameof(WaitMovementEnd));
            }
        }

        private IEnumerator WaitDropOff(DropOffTrigger dropOff, float delay)
        {
            gameManager.ChangeGameState(GameState.Wait);
            Rb.isKinematic = true;
            forceCollider.SetActive(true);

            yield return new WaitForSeconds(delay);

            forceCollider.SetActive(false);

            if (dropOff.isFilled)
            {
                OnLevelProgress?.Invoke();
                Rb.isKinematic = false;
                gameManager.ChangeGameState(GameState.Game);
            }
            else
            {
                OnLevelEnd?.Invoke(false);
            }
        }

        private IEnumerator WaitMovementEnd()
        {
            yield return new WaitForSeconds(4);
            _currentRamp.TileCollidersDisabled();
            Rb.isKinematic = true;
            OnMoneyChange?.Invoke(_endMoney);
            OnLevelEnd?.Invoke(true);
        }
    }
}
