using Zenject;

public class ServicesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        GeneratorInstaller.Install(Container);
        EventInstaller.Install(Container);
    }
}