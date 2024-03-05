using UnityEngine;
using Zenject;

public class DataInstaller : MonoInstaller
{
    [SerializeField] private GameDataManager dataManager;

    public override void InstallBindings()
    {
        Container.Bind<GameDataManager>().FromInstance(dataManager).AsSingle().NonLazy();
    }
}