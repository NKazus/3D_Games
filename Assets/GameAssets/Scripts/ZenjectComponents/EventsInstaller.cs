using Zenject;

public class EventsInstaller : Installer<EventsInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<EventManager>().AsSingle().NonLazy();
    }
}