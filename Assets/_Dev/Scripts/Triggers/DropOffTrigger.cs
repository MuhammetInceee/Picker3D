using TMPro;
using UnityEngine;

namespace Picker.Trigger
{
    public class DropOffTrigger : MonoBehaviour
    {
        private static readonly int Filled = Animator.StringToHash("Filled");
        
        internal bool isFilled;
        
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private int requiredCount;
        
        
        private int _currentCount;

        //Properties
        private string TextContent => text.text = $"{_currentCount} / {requiredCount}";

        private int CurrentCount
        {
            get => _currentCount;
            set
            {
                _currentCount = value;
                text.text = TextContent;

                if (_currentCount >= requiredCount)
                {
                    isFilled = true;
                    animator.SetTrigger(Filled);
                }
            }
        }

        private void Awake()
        {
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
    }
}
