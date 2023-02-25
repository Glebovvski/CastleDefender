using Managers;
using Zenject;

public class AdManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AdManager>().FromNew().AsSingle();
    }
}