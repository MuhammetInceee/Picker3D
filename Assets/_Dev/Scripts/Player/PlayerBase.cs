using Picker.Interfaces;
using Picker.Managers;
using UnityEngine;

namespace Picker.Player
{
    public class PlayerBase : MonoBehaviour, IResetable
    {
        protected float HorizontalSpeed;
        protected GameManager gameManager;
        protected Rigidbody Rb;
        protected PlayerCollision playerCollision;

        protected virtual void Awake()
        {
            gameManager = GameManager.Instance;
            Rb = GetComponent<Rigidbody>();
            playerCollision = GetComponent<PlayerCollision>();
        }
        
        protected void SetRbConstraints(RigidbodyConstraints constraints) => Rb.constraints = constraints;

        public virtual void Reset()
        {
            Rb.isKinematic = false;
            SetRbConstraints(RigidbodyConstraints.FreezeRotation);
        }
    }
}
