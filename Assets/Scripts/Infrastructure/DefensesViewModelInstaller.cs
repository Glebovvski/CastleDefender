using UnityEngine;
using ViewModels;
using Zenject;

public class DefensesViewModelInstaller : MonoInstaller
{
    [SerializeField] private DefensesViewModel viewModel;
    public override void InstallBindings()
    {
        Container.Bind<DefensesViewModel>().FromInstance(viewModel).AsSingle();
    }
}