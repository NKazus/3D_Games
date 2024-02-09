using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "LampColorsInstaller", menuName = "Installers/LampColorsInstaller")]
public class LampColorsInstaller : ScriptableObjectInstaller<LampColorsInstaller>
{
    [SerializeField] private LampColors target;

    public override void InstallBindings()
    {
        Container.Bind<LampColors>().FromScriptableObject(target).AsSingle();
    }
}