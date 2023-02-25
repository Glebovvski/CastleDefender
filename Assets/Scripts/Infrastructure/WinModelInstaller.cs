using Models;
using ViewModels;
using Zenject;

public class WinModelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<WinModel>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<WinViewModel>().FromNew().AsSingle();
    }
}