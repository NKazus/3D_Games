using Zenject;

public class EventsInstaller : Installer<EventsInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<GameGlobalEvents>().AsSingle().NonLazy();
    }
}