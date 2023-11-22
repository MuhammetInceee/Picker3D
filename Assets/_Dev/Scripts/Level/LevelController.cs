using System;
using System.Collections;
using System.Collections.Generic;
using Picker.Extensions;
using UnityEngine;

namespace Picker.Level
{
    public class LevelController : MonoBehaviour
    {
        private static readonly int LockDoor = Animator.StringToHash("LockDoor");
        
        public Transform playerFirstPos;
        
        private List<GameObject> _usedBall = new ();
        
        [SerializeField] private ObjectPoolingSO ballPooling;
        [SerializeField] private List<Transform> ballHolders;
        [SerializeField] private List<Collider> resetableColliders;
        [SerializeField] private List<Animator> dropOffAnimators;

        private void OnEnable()
        {
            for (int i = 0; i < ballHolders.Count; i++)
            {
                GameObject ball = ballPooling.GetPooledObject();
                ball.transform.position = ballHolders[i].position;
                _usedBall.Add(ball);
                ball.SetActive(true);
            }
            
            // foreach (Animator animator in dropOffAnimators)
            // {
            //     animator.SetTrigger(LockDoor);
            // }
        }

        private void OnDisable()
        {
            foreach (GameObject obj in _usedBall)
            {
                obj.tag = "Ball";
                ballPooling.ReturnToPool(obj);
            }

            foreach (Collider col in resetableColliders)
            {
                col.enabled = true;
            }
        }
    }
}
