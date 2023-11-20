using System.Collections.Generic;
using DG.Tweening;
using Picker.Enums;
using Picker.Player;
using Picker.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Picker.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Require Components")] 
        [SerializeField] private PlayerCollision playerCollision;

        [Header("GameScreens")] 
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameObject levelEndScreen;

        [Header("UIElements")] 
        [SerializeField] private Button startTrigger;
        [SerializeField] private Button restartTrigger;

        [Header("Level Progress Bars")] 
        [SerializeField] private List<Image> levelProgressBars;

        private GameManager _gameManager;
        private int _currentLevelStep = 0;

        private void Awake()
        {
            InitVariables();
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            playerCollision.OnLevelEnd += OpenFinishRect;
            playerCollision.OnLevelProgress += LevelProgress;
            startTrigger.onClick.AddListener(TapToPlayTrigger);
            restartTrigger.onClick.AddListener(SceneReload);
        }

        private void TapToPlayTrigger()
        {
            startScreen.SetActive(false);
            gameScreen.SetActive(true);
            _gameManager.ChangeGameState(GameState.Game);
        }

        private void SceneReload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OpenFinishRect()
        {
            gameScreen.SetActive(false);
            levelEndScreen.SetActive(true);
            _gameManager.ChangeGameState(GameState.Wait);
        }

        private void LevelProgress()
        {
            if (_currentLevelStep < 0 || _currentLevelStep >= levelProgressBars.Count) return;
            Color targetColor = new Color(255f / 255f, 135f / 255f, 40f / 255f);
            levelProgressBars[_currentLevelStep].DOColor(targetColor, 0.5f);
            _currentLevelStep++;
        }

        private void InitVariables()
        {
            _gameManager = GameManager.Instance;
        }
    }
}