using UnityEngine;
using Zenject;

public class ResourceInstaller : MonoInstaller
{
    [SerializeField] private ResourceHandler resources;

    public override void InstallBindings()
    {
        Container.Bind<ResourceHandler>().FromInstance(resources).AsSingle().NonLazy();
    }
}