using System;
using Picker.Extensions;
using Picker.Interfaces;
using Picker.Level;
using Picker.Player;
using TMPro;
using UnityEngine;

namespace Picker.Managers
{
    public class LevelManager : MonoBehaviour, IResetable
    {
        private const string LevelKey = "CurrentLevel";
        private const float LevelOffset = 522.6f;

        private GameObject _previousLevel;
        private GameObject _currentLevel;
        private GameManager _gameManager;
        
        [SerializeField] private ObjectPoolingSO allLevels;
        [SerializeField] private PlayerCollision playerCollision;

        [Header("About UI")] 
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private TextMeshProUGUI nextLevelText;
        
        private int _tempLevelCounter;

        private int LevelCounter
        {
            get => PlayerPrefs.GetInt(LevelKey, 0);
            set => PlayerPrefs.SetInt(LevelKey, value);
        }

        private void Awake()
        {
            //First Level Initialized
            _gameManager = GameManager.Instance;
            InitLevel();
            TextUpdater();
            playerCollision.OnRampEnter += SettleNextLevel;
            playerCollision.OnLevelEnd += TextUpdater;
            playerCollision.OnReset += Reset;
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.Space))
        //     {
        //         SettleNextLevel();
        //     }
        // }

        private void OnDisable()
        {
            playerCollision.OnRampEnter -= SettleNextLevel;
            playerCollision.OnLevelEnd -= TextUpdater;
            playerCollision.OnReset -= Reset;
        }

        private void InitLevel(Vector3 targetPos = default)
        {
            GameObject level = allLevels.GetPoolObjectWithName($"{LevelCounter + 1}");
            if (level)
            {
                level.transform.position = targetPos;
                RetainedLevelsModifier(level);
                level.SetActive(true);
                _gameManager.playerFirstPos = level.GetComponent<LevelController>().playerFirstPos;
            }
            else
            {
                GameObject randomLevel = allLevels.GetPoolObjectRandomize();
                randomLevel.transform.position = targetPos;
                RetainedLevelsModifier(randomLevel);
                randomLevel.SetActive(true);
                _gameManager.playerFirstPos = randomLevel.GetComponent<LevelController>().playerFirstPos;
            }
        }

        private void SettleNextLevel()
        {
            LevelCounter++;
            _tempLevelCounter++;
            InitLevel(new Vector3(0, 0, (_tempLevelCounter * LevelOffset)));
        }

        private void DisablePreviousLevel()
        {
            if(_previousLevel == null) return;
            allLevels.ReturnToPool(_previousLevel);
            _previousLevel = null;
        }

        private void RetainedLevelsModifier(GameObject level)
        {
            _previousLevel = _currentLevel;
            _currentLevel = level;
        }

        private void TextUpdater(bool isWin = true)
        {
            currentLevelText.text = $"{LevelCounter + 1}";
            nextLevelText.text = $"{LevelCounter + 2}";
        }

        public void Reset()
        {
            DisablePreviousLevel();
        }
    }
}