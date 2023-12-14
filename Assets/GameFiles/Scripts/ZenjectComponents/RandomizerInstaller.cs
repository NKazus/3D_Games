using Zenject;

public class RandomizerInstaller : Installer<RandomizerInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<Randomizer>().AsSingle().NonLazy();
    }
}