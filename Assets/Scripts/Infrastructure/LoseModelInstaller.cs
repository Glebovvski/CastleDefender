using Models;
using Zenject;

public class LoseModelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<LoseModel>().FromNew().AsSingle();
    }
}