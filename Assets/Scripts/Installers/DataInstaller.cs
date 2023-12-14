using UnityEngine;
using Zenject;
using FitTheSize.GameServices;

public class DataInstaller : MonoInstaller
{
    [SerializeField] GameData data;

    public override void InstallBindings()
    {
        Container.Bind<GameData>().FromInstance(data).AsSingle().NonLazy();
    }

}