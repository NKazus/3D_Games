using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PixieColorsInstaller", menuName = "Installers/PixieColorsInstaller")]
public class PixieColorsInstaller : ScriptableObjectInstaller<PixieColorsInstaller>
{
    [SerializeField] private PixieColors target;
    public override void InstallBindings()
    {
        Container.Bind<PixieColors>().FromScriptableObject(target).AsSingle();
    }
}