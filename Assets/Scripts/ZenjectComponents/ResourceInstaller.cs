using UnityEngine;
using Zenject;

public class ResourceInstaller : MonoInstaller
{
    [SerializeField] private ResourceController resources;

    public override void InstallBindings()
    {
        Container.Bind<ResourceController>().FromInstance(resources).AsSingle().NonLazy();
    }
}