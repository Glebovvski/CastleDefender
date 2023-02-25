using Managers;
using UnityEngine;
using Zenject;

public class PlaneManagerInstaller : MonoInstaller
{
    [SerializeField] private PlaneManager planeManager;
    public override void InstallBindings()
    {
        Container.Bind<PlaneManager>().FromInstance(planeManager).AsSingle();
    }
}