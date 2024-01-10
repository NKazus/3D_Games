using Zenject;

public class ValueInstaller : Installer<ValueInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<ValueProvider>().AsSingle().NonLazy();
    }
}