using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle vibroToggle;

    [Inject] private readonly InGameEvents eventManager;

    #region MONO
    private void OnEnable()
    {
        musicToggle.onValueChanged.AddListener(TurnMisic);
        vibroToggle.onValueChanged.AddListener(TurnVibro);

        musicToggle.isOn = (PlayerPrefs.GetInt("_GameSound") == 1);
        vibroToggle.isOn = (PlayerPrefs.GetInt("_GameVibro") == 1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_GameSound", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("_GameVibro", vibroToggle.isOn ? 1 : 0);

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
