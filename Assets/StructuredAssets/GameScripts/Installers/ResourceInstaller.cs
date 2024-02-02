using UnityEngine;
using Zenject;

public class ResourceInstaller : MonoInstaller
{
    [SerializeField] private AppResourceManager dataHandler;

    public override void InstallBindings()
    {
        Container.Bind<AppResourceManager>().FromInstance(dataHandler).AsSingle().NonLazy();
    }
}