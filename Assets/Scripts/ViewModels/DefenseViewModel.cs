using Models;
using Views;

namespace ViewModels
{
    public class DefenseViewModel
    {
        private CurrencyModel CurrencyModel { get; set; }
        private DefensesModel DefensesModel { get; set; }

        private DefenseView DefenseView { get; set; }
        private ScriptableDefense Defense { get; set; }

        public DefenseViewModel(ScriptableDefense defense, DefenseView view, CurrencyModel currencyModel, DefensesModel defensesModel)
        {
            DefensesModel = defensesModel;
            CurrencyModel = currencyModel;
            Defense = defense;
            DefenseView = view;
            DefenseView.OnDefenseSelected += SelectDefence;
            CurrencyModel.OnGoldAmountChanged += UpdateDefenseAffordable;
        }

        private void UpdateDefenseAffordable(int gold)
        {
            DefenseView.UpdateButton(Defense.Price <= gold);
        }

        public void SelectDefence()
        {
            DefensesModel.DefenseSelected(Defense);
        }
    }
}