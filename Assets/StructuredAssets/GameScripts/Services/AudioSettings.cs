using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle vibroToggle;

    [Inject] private readonly AppEvents eventManager;

    private void OnEnable()
    {
        musicToggle.onValueChanged.AddListener(TurnMisic);
        vibroToggle.onValueChanged.AddListener(TurnVibro);

        musicToggle.isOn = (PlayerPrefs.GetInt("App_Sound_On") == 1);
        vibroToggle.isOn = (PlayerPrefs.GetInt("App_Vibro_On") == 1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("App_Sound_On", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("App_Vibro_On", vibroToggle.isOn ? 1 : 0);

        musicToggle.onValueChanged.RemoveListener(TurnMisic);
        vibroToggle.onValueChanged.RemoveListener(TurnVibro);
    }

    private void TurnMisic(bool isMusicOn)
    {
        eventManager.SetSound(isMusicOn);
    }

    private void TurnVibro(bool isVibroOn)
    {
        eventManager.SetVibro(isVibroOn);
    }
}
