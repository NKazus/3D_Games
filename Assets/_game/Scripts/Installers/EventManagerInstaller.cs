using Zenject;

public class EventManagerInstaller : Installer<EventManagerInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<GlobalEventManager>().AsSingle().NonLazy();
    }
}