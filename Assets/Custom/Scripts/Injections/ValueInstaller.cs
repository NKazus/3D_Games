using Zenject;

public class ValueInstaller : Installer<ValueInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IntGenerator>().AsSingle().NonLazy();
    }
}