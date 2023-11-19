using TMPro;
using UnityEngine;

namespace Picker.Trigger
{
    public class DropOffTrigger : MonoBehaviour
    {
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
                    //TODO platform animation will be added
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