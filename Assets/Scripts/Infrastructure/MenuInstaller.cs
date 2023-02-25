using ViewModels;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<MenuViewModel>().FromNew().AsSingle();
    }
}