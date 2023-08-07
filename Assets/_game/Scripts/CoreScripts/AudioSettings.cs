using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle vibroToggle;

    [Inject] private readonly GlobalEventManager eventManager;

    #region MONO
    private void OnEnable()
    {
        musicToggle.onValueChanged.AddListener(TurnMisic);
        vibroToggle.onValueChanged.AddListener(TurnVibro);

        if (!PlayerPrefs.HasKey("_SoundEnabled"))
        {
            PlayerPrefs.SetInt("_SoundEnabled", 1);
            PlayerPrefs.SetInt("_VibroEnabled", 1);
        }
        musicToggle.isOn = (PlayerPrefs.GetInt("_SoundEnabled") == 1);
        vibroToggle.isOn = (PlayerPrefs.GetInt("_VibroEnabled") == 1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_SoundEnabled", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("_VibroEnabled", vibroToggle.isOn ? 1 : 0);

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
