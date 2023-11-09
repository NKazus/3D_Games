using UnityEngine;
using Zenject;

public class InputInstaller : MonoInstaller
{
    [SerializeField] private GameInput inputComponent;

    public override void InstallBindings()
    {
        Container.Bind<GameInput>().FromInstance(inputComponent).AsSingle().NonLazy();
    }
}