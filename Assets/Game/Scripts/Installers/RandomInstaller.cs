using Zenject;

public class RandomInstaller : Installer<RandomInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<RandomGenerator>().AsSingle().NonLazy();
    }
}