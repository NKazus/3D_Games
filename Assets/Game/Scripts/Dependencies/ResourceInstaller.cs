using UnityEngine;
using Zenject;

public class ResourceInstaller : MonoInstaller
{
    [SerializeField] private GameResourceHandler resources;

    public override void InstallBindings()
    {
        Container.Bind<GameResourceHandler>().FromInstance(resources).AsSingle().NonLazy();
    }
}