using Zenject;

public class NonMonobehaviorInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        RandomInstaller.Install(Container);
        EventInstaller.Install(Container);
    }
}