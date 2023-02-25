using Managers;
using Models;
using UnityEngine;
using ViewModels;
using Views;
using Zenject;

public class DefenseViewFactory : PlaceholderFactory<DefenseView>
{
    private CurrencyModel CurrencyModel { get; set; }
    private DefensesModel DefensesModel { get; set; }
    private AudioManager AudioManager { get; set; }


    [Inject]
    private void Construct(CurrencyModel currencyModel, DefensesModel defensesModel, AudioManager audioManager)
    {
        CurrencyModel = currencyModel;
        DefensesModel = defensesModel;
        AudioManager = audioManager;
    }

    public DefenseView CreateDefenseView(ScriptableDefense defense, DefenseView prefab, Transform parent)
    {
        if (defense.Price == 0) return null;

        var view = GameObject.Instantiate(prefab, parent);
        DefenseViewModel vm = new DefenseViewModel(defense, view, CurrencyModel, DefensesModel);
        view.Init(defense.Price.ToString(), defense.Image, AudioManager);
        return view;
    }
}
