using System;
using Models;

namespace ViewModels
{
    public class GoldViewModel
    {
        private CurrencyModel CurrencyModel { get; set; }

        public event Action<int> OnGoldChanged;

        public GoldViewModel(CurrencyModel currencyModel)
        {
            CurrencyModel = currencyModel;
            CurrencyModel.OnGoldAmountChanged += GoldChanged;
        }

        public string GetGoldPercent() => string.Format("+{0}% / {1}s", CurrencyModel.GoldPercent, CurrencyModel.SecondsToDropGold);

        private void GoldChanged(int value) => OnGoldChanged?.Invoke(value);
    }
}