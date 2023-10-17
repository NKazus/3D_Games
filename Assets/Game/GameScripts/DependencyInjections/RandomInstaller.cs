using Zenject;

public class RandomInstaller : Installer<RandomInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<RandomValueProvider>().AsSingle().NonLazy();
    }
}