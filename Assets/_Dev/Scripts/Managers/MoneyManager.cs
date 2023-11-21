using System;
using Sirenix.OdinInspector;
using System.Collections;
using Picker.Player;
using TMPro;
using UnityEngine;

namespace Picker.Managers
{
    public class MoneyManager : MonoBehaviour
    {
        public Action OnMoneyChange;

        public int currentMoney;
        [SerializeField] private TextMeshProUGUI moneyText;

        [Header("Require Components")] 
        [SerializeField] private UIManager uiManager;
        [SerializeField] private PlayerCollision playerCollision;

        private void Awake()
        {
            currentMoney = PlayerPrefs.GetInt("Money", 0);
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => uiManager);
            MoneyTextUpdate();
        }

        private void OnEnable() => SubscribeEvents();
        
        private void OnDisable()
        {
            SetPlayerPrefs();
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            playerCollision.OnMoneyChange += IncreaseMoney;
        }

        private void UnsubscribeEvents()
        {
            playerCollision.OnMoneyChange -= IncreaseMoney;
        }

        [Button]
        public bool ChangeMoney(int money)
        {
            if (!CheckCanBuy(money))
            {
                return false;
            }

            currentMoney += money;
            SetPlayerPrefs();
            MoneyTextUpdate();
            OnMoneyChange?.Invoke();
            return true;
        }

        public bool CheckCanBuy(float money)
        {
            return !(currentMoney + money < 0);
        }

        public void SetPlayerPrefs() => PlayerPrefs.SetInt("Money", currentMoney);
        
        public void IncreaseMoney(int money) => ChangeMoney(money);
        
        public void DecreaseMoney(int money) => ChangeMoney(-money);
        
        private void MoneyTextUpdate()
        {
            moneyText.text = $"{currentMoney}";
        }
    }
}