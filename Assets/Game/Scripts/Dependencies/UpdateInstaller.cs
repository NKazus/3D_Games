using UnityEngine;
using Zenject;

public class UpdateInstaller : MonoInstaller
{
    [SerializeField] private UpdateManager updateManager;

    public override void InstallBindings()
    {
        Container.Bind<UpdateManager>().FromInstance(updateManager).AsSingle().NonLazy();
    }
}