using UnityEngine;
using Zenject;

public class UpdateInstaller : MonoInstaller
{
    [SerializeField] GlobalUpdate updateInstance;

    public override void InstallBindings()
    {
        Container.Bind<GlobalUpdate>().FromInstance(updateInstance).AsSingle().NonLazy();
    }
}