using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Picker.Trigger
{
    public class DropOffTrigger : MonoBehaviour
    {
        internal bool isFilled;
        
        [SerializeField] private int requiredCount;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Animator animator;
        
        private int _currentCount;

        private int CurrentCount
        {
            get => _currentCount;
            set
            {
                _currentCount = value;
                text.text = $"{_currentCount} / {requiredCount}";

                if (_currentCount >= requiredCount)
                {
                    isFilled = true;
                    //TODO platform animation will be added
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                _currentCount++;
                other.tag = "Untagged";
            }
        }
    }
}
