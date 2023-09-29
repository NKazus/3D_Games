using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "WallParamsInstaller", menuName = "Installers/WallParamsInstaller")]
public class WallParamsInstaller : ScriptableObjectInstaller<WallParamsInstaller>
{
    [SerializeField] private WallParams target;
    public override void InstallBindings()
    {
        Container.Bind<WallParams>().FromScriptableObject(target).AsSingle();
    }
}