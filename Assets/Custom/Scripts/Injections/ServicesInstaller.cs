using Zenject;

public class ServicesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        EventsInstaller.Install(Container);
        ValueInstaller.Install(Container);
    }
}