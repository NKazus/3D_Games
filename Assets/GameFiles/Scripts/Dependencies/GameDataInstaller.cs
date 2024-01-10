using UnityEngine;
using Zenject;

public class GameDataInstaller : MonoInstaller
{
    [SerializeField] private GameData data;

    public override void InstallBindings()
    {
        Container.Bind<GameData>().FromInstance(data).AsSingle().NonLazy();
    }

}