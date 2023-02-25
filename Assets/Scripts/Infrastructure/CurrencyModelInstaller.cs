using Models;
using ViewModels;
using Zenject;

public class CurrencyModelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        CurrencyModel currencyModel = new CurrencyModel();
        Container.Bind<CurrencyModel>().FromNew().AsSingle();
        
        GoldViewModel goldViewModel = new GoldViewModel(currencyModel);
        Container.Bind<GoldViewModel>().FromNew().AsSingle();
    }
}