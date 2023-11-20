using Picker.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Picker.Player
{
    public class PlayerThrowController : PlayerBase
    {
        private PlayerController _playerController;
        
        [SerializeField] private Slider forceSlider;
        [SerializeField] private TextMeshProUGUI percentText;

        protected override void Awake()
        {
            base.Awake();
            _playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if(gameManager.gameState != GameState.Ramp) return;

            _playerController.SpeedModifier = forceSlider.value / 5;
            percentText.text = $"%{(int)(forceSlider.value)}";
            forceSlider.value = 
                Input.GetMouseButtonDown(0) ? Mathf.Clamp(forceSlider.value + 10, 0f, 100f) : Mathf.Clamp(forceSlider.value - 0.10f, 0f, 100f);
        }
    }
}
