using Zenject;

public class SystemInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        EventsInstaller.Install(Container);
        RandomizerInstaller.Install(Container);
    }
}