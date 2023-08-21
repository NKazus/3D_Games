using UnityEngine;
using Zenject;

public class DataInstaller : MonoInstaller
{
    [SerializeField] GameData data;

    public override void InstallBindings()
    {
        Container.Bind<GameData>().FromInstance(data).AsSingle().NonLazy();
    }

}