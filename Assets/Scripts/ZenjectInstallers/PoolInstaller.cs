using Zenject;
using FitTheSize.GameServices;

public class PoolInstaller : Installer<PoolInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<GamePool>().AsSingle();
    }
}