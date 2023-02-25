using System;
using UnityEngine;

namespace Models
{
    public class CurrencyModel
    {
        public float SecondsToDropGold { get; private set; } = 5f;
        public float GoldPercent { get; private set; } = 2f;

        public event Action<int> OnGoldAmountChanged;
        private int _gold;
        public int Gold
        {
            get => _gold;
            private set
            {
                _gold = value;
                PlayerPrefs.SetInt("Gold", value);
                OnGoldAmountChanged?.Invoke(_gold);
            }
        }

        public CurrencyModel()
        {
            Gold = PlayerPrefs.GetInt("Gold", 10000);
        }

        public bool TryBuy(int price)
        {
            if (price > Gold)
                return false;

            Gold -= price;
            return true;
        }

        public void AddGold(int value)
        {
            Gold += value;
        }
    }
}