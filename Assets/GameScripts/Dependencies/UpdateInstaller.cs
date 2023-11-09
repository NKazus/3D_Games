using UnityEngine;
using Zenject;

public class UpdateInstaller : MonoInstaller
{
    [SerializeField] private GameUpdateHandler updateHandler;

    public override void InstallBindings()
    {
        Container.Bind<GameUpdateHandler>().FromInstance(updateHandler).AsSingle().NonLazy();
    }
}