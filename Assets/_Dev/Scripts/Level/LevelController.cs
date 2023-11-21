using System;
using System.Collections;
using System.Collections.Generic;
using Picker.Extensions;
using UnityEngine;

namespace Picker.Level
{
    public class LevelController : MonoBehaviour
    {
        private List<GameObject> _usedBall = new ();
        
        [SerializeField] private ObjectPoolingSO ballPooling;
        [SerializeField] private List<Transform> ballHolders;

        private void OnEnable()
        {
            for (int i = 0; i < ballHolders.Count; i++)
            {
                GameObject ball = ballPooling.GetPooledObject();
                ball.transform.position = ballHolders[i].position;
                _usedBall.Add(ball);
                ball.SetActive(true);
            }
        }

        private void OnDisable()
        {
            foreach (GameObject obj in _usedBall)
            {
                ballPooling.ReturnToPool(obj);
            }
        }
    }
}
