using UnityEngine;
using Zenject;

public class ResourceInstaller : MonoInstaller
{
    [SerializeField] private Resources resources;

    public override void InstallBindings()
    {
        Container.Bind<Resources>().FromInstance(resources).AsSingle().NonLazy();
    }
}