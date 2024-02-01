using Zenject;

public class GeneratorInstaller : Installer<GeneratorInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<ValueGenerator>().AsSingle().NonLazy();
    }
}