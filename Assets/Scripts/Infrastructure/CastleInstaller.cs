using Defendable;
using UnityEngine;
using Zenject;

public class CastleInstaller : MonoInstaller
{
    [SerializeField] private CastleDefense castlePrefab;

    public override void InstallBindings()
    {
        var castle = Instantiate(castlePrefab, Vector3.zero, Quaternion.identity);
        Container.Bind<CastleDefense>().FromInstance(castle).AsSingle();
        castle.gameObject.SetActive(false);
    }
}