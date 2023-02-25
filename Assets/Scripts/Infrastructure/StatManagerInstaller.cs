using Managers;
using Zenject;

public class StatManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<StatManager>().FromNew().AsSingle();
    }
}