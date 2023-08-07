using UnityEngine;
using Zenject;

public class DataHandlerInstaller : MonoInstaller
{
    [SerializeField] private DataHandler dataHandler;
    public override void InstallBindings()
    {
        Container.Bind<DataHandler>().FromInstance(dataHandler).AsSingle().NonLazy();
    }
}