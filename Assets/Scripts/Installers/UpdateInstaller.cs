using UnityEngine;
using Zenject;
using FitTheSize.GameServices;

public class UpdateInstaller : MonoInstaller
{
    [SerializeField] GameUpdateHandler update;

    public override void InstallBindings()
    {
        Container.Bind<GameUpdateHandler>().FromInstance(update).AsSingle().NonLazy();
    }
}