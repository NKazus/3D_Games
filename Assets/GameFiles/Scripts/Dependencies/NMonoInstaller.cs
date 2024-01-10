using Zenject;

public class NMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        EventInstaller.Install(Container);
        ValueInstaller.Install(Container);
    }
}