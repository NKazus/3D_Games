using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameAudioSettings : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle vibroToggle;

    [Inject] private readonly GameEvents events;

    #region MONO
    private void OnEnable()
    {
        musicToggle.onValueChanged.AddListener(TurnMisic);
        vibroToggle.onValueChanged.AddListener(TurnVibro);

        musicToggle.isOn = (PlayerPrefs.GetInt("AppSettings_Sound") == 1);
        vibroToggle.isOn = (PlayerPrefs.GetInt("AppSettings_Vibro") == 1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("AppSettings_Sound", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("AppSettings_Vibro", vibroToggle.isOn ? 1 : 0);

        musicToggle.onValueChanged.RemoveListener(TurnMisic);
        vibroToggle.onValueChanged.RemoveListener(TurnVibro);
    }
    #endregion

    private void TurnMisic(bool isMusicOn)
    {
        events.SetSound(isMusicOn);
    }

    private void TurnVibro(bool isVibroOn)
    {
        events.SetVibro(isVibroOn);
    }
}
