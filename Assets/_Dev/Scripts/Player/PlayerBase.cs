using Picker.Managers;
using UnityEngine;

namespace Picker.Player
{
    public class PlayerBase : MonoBehaviour
    {
        protected float HorizontalSpeed;
        protected GameManager gameManager;
        protected Rigidbody Rb;

        protected virtual void Awake()
        {
            gameManager = GameManager.Instance;
            Rb = GetComponent<Rigidbody>();
        }
        
        protected void SetRbConstraints(RigidbodyConstraints constraints) => Rb.constraints = constraints;
    }
}
