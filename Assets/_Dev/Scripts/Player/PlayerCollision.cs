using System;
using System.Collections;
using DG.Tweening;
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
        public event Action OnReset;
        public event Action OnLevelProgress;
        public event Action OnRampEnter;
        public event Action OnThrow;
        
        [SerializeField] private GameObject forceCollider;
        [SerializeField] private GameObject extraColliders;

        private Ramp _currentRamp;
        private int _endMoney;

        private void OnEnable()
        {
            OnReset += Reset;
        }

        private void OnDisable()
        {
            OnReset -= Reset;
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
                _currentRamp = other.GetComponent<Ramp>();
                gameManager.ChangeGameState(GameState.Ramp);
                OnRampEnter?.Invoke();
                other.enabled = false;
                SetRbConstraints(RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ);
                extraColliders.SetActive(false);
                
                HorizontalSpeed = 0;
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
            _currentRamp.TileCollidersControl(false);
            Rb.isKinematic = true;
            OnMoneyChange?.Invoke(_endMoney);
            Sequence playerMovementSequence = DOTween.Sequence();

            Tween moveUpTween = transform.DOMoveY(1, 0.2f)
                .SetRelative()
                .SetEase(Ease.Linear);
            Tween verticalMoveTween = transform.DOMove(gameManager.playerFirstPos.position, 1f)
                .SetEase(Ease.Linear);
            Tween rotateTween = transform.DORotate(Vector3.zero, 1f);

            playerMovementSequence.Insert(0, moveUpTween);
            playerMovementSequence.Insert(0.2f, verticalMoveTween);
            playerMovementSequence.Insert(0.2f, rotateTween);

            playerMovementSequence.Play()
                .OnComplete(() =>
                {
                    OnLevelEnd?.Invoke(true);
                    OnReset?.Invoke();
                });
        }

        public override void Reset()
        {
            base.Reset();
            extraColliders.SetActive(true);
        }
    }
}
