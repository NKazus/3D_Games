using UnityEngine;
using Zenject;

public class UpdateInstaller : MonoInstaller
{
    [SerializeField] private UpdateController updateManager;

    public override void InstallBindings()
    {
        Container.Bind<UpdateController>().FromInstance(updateManager).AsSingle().NonLazy();
    }
}