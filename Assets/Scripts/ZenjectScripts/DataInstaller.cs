using UnityEngine;
using Zenject;

public class DataInstaller : MonoInstaller
{
    [SerializeField] DataHandler data;

    public override void InstallBindings()
    {
        Container.Bind<DataHandler>().FromInstance(data).AsSingle().NonLazy();
    }
}