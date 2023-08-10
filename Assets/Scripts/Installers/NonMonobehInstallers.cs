using Zenject;

public class NonMonobehInstallers : MonoInstaller
{
    public override void InstallBindings()
    {
        EventManagerInstaller.Install(Container);
    }
}