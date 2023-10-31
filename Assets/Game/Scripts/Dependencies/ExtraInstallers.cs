using Zenject;

public class ExtraInstallers : MonoInstaller
{
    public override void InstallBindings()
    {
        EventsInstaller.Install(Container);
        RandomInstaller.Install(Container);
    }
}