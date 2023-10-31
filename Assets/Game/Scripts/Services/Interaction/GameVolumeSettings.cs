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

        volumeToggle.isOn = (PlayerPrefs.GetInt("_GameAudio_Volume") == 1);
        vibroToggle.isOn = (PlayerPrefs.GetInt("_GameAudio_Vibro") == 1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_GameAudio_Volume", volumeToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("_GameAudio_Vibro", vibroToggle.isOn ? 1 : 0);

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
