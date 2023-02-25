using Managers;
using UnityEngine;
using Zenject;

public class AudioManagerInstaller : MonoInstaller
{
    [SerializeField] private AudioManager audioManager;
    public override void InstallBindings()
    {
        Container.Bind<AudioManager>().FromInstance(audioManager);
    }
}