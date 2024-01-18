using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle vibroToggle;

    [Inject] private readonly EventHandler events;


    private void OnEnable()
    {
        musicToggle.onValueChanged.AddListener(TurnMisic);
        vibroToggle.onValueChanged.AddListener(TurnVibro);

        musicToggle.isOn = (PlayerPrefs.GetInt("SETTINGS_INT_SoundEnabled") == 1);
        vibroToggle.isOn = (PlayerPrefs.GetInt("SETTINGS_INT_VibroEnabled") == 1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("SETTINGS_INT_SoundEnabled", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("SETTINGS_INT_VibroEnabled", vibroToggle.isOn ? 1 : 0);

        musicToggle.onValueChanged.RemoveListener(TurnMisic);
        vibroToggle.onValueChanged.RemoveListener(TurnVibro);
    }


    private void TurnMisic(bool isMusicOn)
    {
        events.SetSound(isMusicOn);
    }

    private void TurnVibro(bool isVibroOn)
    {
        events.SetVibro(isVibroOn);
    }
}
