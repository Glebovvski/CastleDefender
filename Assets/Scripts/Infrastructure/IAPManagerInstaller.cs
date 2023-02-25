using Managers;
using Zenject;

public class IAPManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<IAPManager>().FromNew().AsSingle();
    }
}