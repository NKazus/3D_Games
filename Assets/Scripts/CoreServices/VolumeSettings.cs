using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle vibroToggle;

    [Inject] private readonly InGameEvents eventManager;

    #region MONO
    private void OnEnable()
    {
        musicToggle.onValueChanged.AddListener(TurnMisic);
        vibroToggle.onValueChanged.AddListener(TurnVibro);

        musicToggle.isOn = (PlayerPrefs.GetInt("_InGameVolume") == 1);
        vibroToggle.isOn = (PlayerPrefs.GetInt("_InGameVibro") == 1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_InGameVolume", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("_InGameVibro", vibroToggle.isOn ? 1 : 0);

        musicToggle.onValueChanged.RemoveListener(TurnMisic);
        vibroToggle.onValueChanged.RemoveListener(TurnVibro);
    }
    #endregion

    private void TurnMisic(bool isMusicOn)
    {
        eventManager.SetSound(isMusicOn);
    }

    private void TurnVibro(bool isVibroOn)
    {
        eventManager.SetVibro(isVibroOn);
    }
}
