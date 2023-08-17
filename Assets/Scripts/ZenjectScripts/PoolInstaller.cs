using Zenject;

public class PoolInstaller : Installer<PoolInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<Pool>().AsSingle().NonLazy();
    }
}