using Picker.Extensions;
using UnityEngine;

namespace Picker.Managers
{
    public class LevelManager : MonoBehaviour
    {
        private const string LevelKey = "CurrentLevel";
        private const float LevelOffset = 522.6f;

        //TODO Don't forget to change these variables to internal after the checks.
        public GameObject previousLevel;
        public GameObject currentLevel;

        [SerializeField] private ObjectPoolingSO allLevels;

        private int _tempLevelCounter;

        private int LevelCounter
        {
            get => PlayerPrefs.GetInt(LevelKey, 0);
            set => PlayerPrefs.SetInt(LevelKey, value);
        }

        private void Awake()
        {
            allLevels.InitializeObjectPool(this);
            //First Level Initialized
            InitLevel();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SettleNextLevel();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                DisablePreviousLevel();
            }
        }

        private void InitLevel(Vector3 targetPos = default)
        {
            GameObject level = allLevels.GetPoolObjectWithName($"{LevelCounter + 1}");
            if (level)
            {
                level.transform.position = targetPos;
                RetainedLevelsModifier(level);
                level.SetActive(true);
            }
            else
            {
                GameObject randomLevel = allLevels.GetPoolObjectRandomize();
                randomLevel.transform.position = targetPos;
                RetainedLevelsModifier(randomLevel);
                randomLevel.SetActive(true);
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
            if(previousLevel == null) return;
            allLevels.ReturnToPool(previousLevel);
            previousLevel = null;
        }

        private void RetainedLevelsModifier(GameObject level)
        {
            previousLevel = currentLevel;
            currentLevel = level;
        }
    }
}