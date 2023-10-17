using UnityEngine;
using Zenject;

public class ResourcesInstaller : MonoInstaller
{
    [SerializeField] InGameResources data;

    public override void InstallBindings()
    {
        Container.Bind<InGameResources>().FromInstance(data).AsSingle().NonLazy();
    }
}