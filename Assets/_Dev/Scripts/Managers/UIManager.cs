using System;
using System.Collections.Generic;
using DG.Tweening;
using Picker.Enums;
using Picker.Interfaces;
using Picker.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Picker.Managers
{
    public class UIManager : MonoBehaviour, IResetable
    {
        // public static event Action OnReset;
        
        [Header("Require Components")] 
        [SerializeField] private PlayerCollision playerCollision;

        [Header("GameScreens")] 
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameObject failScreen;
        [SerializeField] private GameObject successScreen;

        [Header("UIElements")] 
        [SerializeField] private Button startTrigger;
        [SerializeField] private Button restartTrigger;

        [Header("Level Progress Bars")] 
        [SerializeField] private List<Image> levelProgressBars;
        [SerializeField] private GameObject throwUI;

        private GameManager _gameManager;
        private int _currentLevelStep = 0;

        private void Awake()
        {
            InitVariables();
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void SubscribeEvents()
        {
            playerCollision.OnLevelEnd += OpenFinishRect;
            playerCollision.OnLevelProgress += LevelProgress;
            playerCollision.OnRampEnter += ThrowUIEnabled;
            playerCollision.OnThrow += ThrowUIDisabled;
            playerCollision.OnReset += Reset;
            startTrigger.onClick.AddListener(TapToPlayTrigger);
            restartTrigger.onClick.AddListener(SceneReload);
        }

        private void UnSubscribeEvents()
        {
            playerCollision.OnLevelEnd -= OpenFinishRect;
            playerCollision.OnLevelProgress -= LevelProgress;
            playerCollision.OnRampEnter -= ThrowUIEnabled;
            playerCollision.OnThrow -= ThrowUIDisabled;
            playerCollision.OnReset -= Reset;
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

        private void OpenFinishRect(bool isSuccess)
        {
            gameScreen.SetActive(false);
            _gameManager.ChangeGameState(GameState.Wait);

            GameObject targetScreen = isSuccess ? successScreen : failScreen;
            targetScreen.SetActive(true);
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

        private void ThrowUIEnabled() => throwUI.SetActive(true);
        private void ThrowUIDisabled() => throwUI.SetActive(false);
        public void Reset()
        {
            ThrowUIDisabled();
            successScreen.SetActive(false);
            startScreen.SetActive(true);

            foreach (Image image in levelProgressBars)
            {
                image.DOColor(Color.white, 0f);
            }
            
            _currentLevelStep = 0;
        }
    }
}