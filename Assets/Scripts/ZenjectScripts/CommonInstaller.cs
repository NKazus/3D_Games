using Zenject;

public class CommonInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        EventInstaller.Install(Container);
        RandomInstaller.Install(Container);
    }
}