using Grid;
using UnityEngine;
using Zenject;

public class GridInstaller : MonoInstaller
{
    [SerializeField] private GameGrid grid;
    
    public override void InstallBindings()
    {
        Container.Bind<GameGrid>().FromInstance(grid).AsSingle();
    }
}