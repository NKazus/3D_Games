using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameVolumeSettings : MonoBehaviour
{
    [SerializeField] private Toggle volumeToggle;
    [SerializeField] private Toggle vibroToggle;

    [Inject] private readonly GameGlobalEvents eventManager;

    #region MONO
    private void OnEnable()
    {
        volumeToggle.onValueChanged.AddListener(SwitchVolume);
        vibroToggle.onValueChanged.AddListener(SwitchVibro);

        volumeToggle.isOn = (PlayerPrefs.GetInt("BOOL_VolumeValue") == 1);
        vibroToggle.isOn = (PlayerPrefs.GetInt("BOOL_VibroValue") == 1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("BOOL_VolumeValue", volumeToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("BOOL_VibroValue", vibroToggle.isOn ? 1 : 0);

        volumeToggle.onValueChanged.RemoveListener(SwitchVolume);
        vibroToggle.onValueChanged.RemoveListener(SwitchVibro);
    }
    #endregion

    private void SwitchVolume(bool isVolumeOn)
    {
        eventManager.SetVolume(isVolumeOn);
    }

    private void SwitchVibro(bool isVibroOn)
    {
        eventManager.SetVibro(isVibroOn);
    }
}
