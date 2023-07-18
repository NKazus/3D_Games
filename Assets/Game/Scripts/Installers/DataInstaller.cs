using UnityEngine;
using Zenject;

public class DataInstaller : MonoInstaller
{
    [SerializeField] private GameDataHandler dataHandler;

    public override void InstallBindings()
    {
        Container.Bind<GameDataHandler>().FromInstance(dataHandler).AsSingle().NonLazy();
    }
}