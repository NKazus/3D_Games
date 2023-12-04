using Zenject;
using FitTheSize.GameServices;

public class EventsInstaller : Installer<EventsInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<GameEventHandler>().AsSingle().NonLazy();
    }
}