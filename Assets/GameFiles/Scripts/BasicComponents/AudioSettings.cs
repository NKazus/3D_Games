using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle vibroToggle;

    [Inject] private readonly GlobalEventManager events;

    #region MONO
    private void OnEnable()
    {
        musicToggle.onValueChanged.AddListener(TurnMisic);
        vibroToggle.onValueChanged.AddListener(TurnVibro);

        musicToggle.isOn = (PlayerPrefs.GetInt("_SoundOn") == 1);
        vibroToggle.isOn = (PlayerPrefs.GetInt("_VibroOn") == 1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_SoundOn", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("_VibroOn", vibroToggle.isOn ? 1 : 0);

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
