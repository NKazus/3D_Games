using Zenject;

public class EventInstaller : Installer<EventInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<GlobalEventManager>().AsSingle().NonLazy();
    }
}