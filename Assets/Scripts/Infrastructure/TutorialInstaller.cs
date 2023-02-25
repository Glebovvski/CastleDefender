using Models;
using ViewModels;
using Zenject;

public class TutorialInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TutorialModel>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<TutorialViewModel>().FromNew().AsSingle();
    }
}