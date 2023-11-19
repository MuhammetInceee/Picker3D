using System;
using UnityEngine;

namespace Picker.Player
{
    public class PlayerCollision : MonoBehaviour
    {
        [SerializeField] private GameObject forceCollider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball") && other.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(Vector3.forward * 15);
            }
        }
    }
}
