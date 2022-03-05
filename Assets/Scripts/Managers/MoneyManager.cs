using System;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class MoneyManager : MonoBehaviour
    {
        public static MoneyManager Instance;
        public UnityEvent<int> onMoneyChanged;
        public int currentMoney { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
                onMoneyChanged ??= new UnityEvent<int>();
                currentMoney = 0;
                onMoneyChanged.Invoke(currentMoney);
            }
        }

        public void AddMoney(int amount)
        {
            currentMoney += amount;
            onMoneyChanged.Invoke(currentMoney);
        } 
    }
}