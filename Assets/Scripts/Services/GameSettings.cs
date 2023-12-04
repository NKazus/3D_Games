using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FitTheSize.GameServices
{
    public class GameSettings : MonoBehaviour
    {
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle vibroToggle;

        [Inject] private readonly GameEventHandler events;

        #region MONO
        private void OnEnable()
        {
            musicToggle.onValueChanged.AddListener(SwitchMusic);
            vibroToggle.onValueChanged.AddListener(SwitchVibro);

            musicToggle.isOn = (PlayerPrefs.GetInt("SETTINGS_MUSIC") == 1);
            vibroToggle.isOn = (PlayerPrefs.GetInt("SETTINGS_VIBRATION") == 1);
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("SETTINGS_MUSIC", musicToggle.isOn ? 1 : 0);
            PlayerPrefs.SetInt("SETTINGS_VIBRATION", vibroToggle.isOn ? 1 : 0);

            musicToggle.onValueChanged.RemoveListener(SwitchMusic);
            vibroToggle.onValueChanged.RemoveListener(SwitchVibro);
        }
        #endregion

        private void SwitchMusic(bool isMusicOn)
        {
            events.SetSound(isMusicOn);
        }

        private void SwitchVibro(bool isVibroOn)
        {
            events.SetVibro(isVibroOn);
        }
    }
}
