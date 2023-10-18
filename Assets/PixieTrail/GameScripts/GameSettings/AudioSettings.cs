using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Toggle volumeToggle;
    [SerializeField] private Toggle vibrationToggle;

    [Inject] private readonly InGameEvents events;

    #region MONO
    private void OnEnable()
    {
        volumeToggle.onValueChanged.AddListener(SwitchMisic);
        vibrationToggle.onValueChanged.AddListener(SwitchVibration);

        volumeToggle.isOn = (PlayerPrefs.GetInt("_PT_VolumeEnabled") == 1);
        vibrationToggle.isOn = (PlayerPrefs.GetInt("_PT_VibrationEnabled") == 1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_PT_VolumeEnabled", volumeToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("_PT_VibrationEnabled", vibrationToggle.isOn ? 1 : 0);

        volumeToggle.onValueChanged.RemoveListener(SwitchMisic);
        vibrationToggle.onValueChanged.RemoveListener(SwitchVibration);
    }
    #endregion

    private void SwitchMisic(bool isMusicOn)
    {
        events.SetSound(isMusicOn);
    }

    private void SwitchVibration(bool isVibroOn)
    {
        events.SetVibro(isVibroOn);
    }
}
