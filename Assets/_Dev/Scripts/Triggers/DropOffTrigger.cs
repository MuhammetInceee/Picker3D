using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Picker.Trigger
{
    public class DropOffTrigger : MonoBehaviour
    {
        private static readonly int Filled = Animator.StringToHash("Filled");

        private int _requireCount;
        
        internal bool isFilled;
        
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI text;
        [Tooltip("The value on the left shows the minimum value it can get, and the value on the right shows the maximum value it can get.")]
        [SerializeField] private Vector2 requiredCount;
        
        private int _currentCount;

        //Properties
        private string TextContent => text.text = $"{_currentCount} / {_requireCount}";

        private int CurrentCount
        {
            get => _currentCount;
            set
            {
                _currentCount = value;
                text.text = TextContent;

                if (_currentCount >= _requireCount)
                {
                    isFilled = true;
                    animator.SetTrigger(Filled);
                }
            }
        }

        private void OnEnable()
        {
            _requireCount = (int)Random.Range(requiredCount.x, requiredCount.y);
            text.text = TextContent;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                CurrentCount++;
                other.tag = "Untagged";
            }
        }

        private void OnDisable()
        {
            CurrentCount = 0;
        }
    }
}
