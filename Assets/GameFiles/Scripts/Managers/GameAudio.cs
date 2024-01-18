using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public enum GameSound
{
    Win,
    Lose,
    Check,
    Spice,
    Unlock
}

public class GameAudio : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource loseSound;
    [SerializeField] private AudioSource checkSound;
    [SerializeField] private AudioSource spiceSound;
    [SerializeField] private AudioSource unlockSound;

    private bool vibroEnabled = true;

    [Inject] private readonly EventHandler events;

    private void OnEnable()
    {
        events.VibroEvent += PlayVibro;
        events.SoundEvent += PlaySound;

        events.VibroSettingsEvent += TurnVibration;
        events.SoundSettingsEvent += TurnSound;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("SETTINGS_INT_SoundEnabled"))
        {
            PlayerPrefs.SetInt("SETTINGS_INT_SoundEnabled", 1);
            PlayerPrefs.SetInt("SETTINGS_INT_VibroEnabled", 1);
        }

        TurnSound(PlayerPrefs.GetInt("SETTINGS_INT_SoundEnabled") == 1);
        TurnVibration(PlayerPrefs.GetInt("SETTINGS_INT_VibroEnabled") == 1);
    }

    private void OnDisable()
    {
        events.VibroEvent -= PlayVibro;
        events.SoundEvent -= PlaySound;

        events.VibroSettingsEvent -= TurnVibration;
        events.SoundSettingsEvent -= TurnSound;
    }


    private void PlayVibro()
    {
        if (vibroEnabled)
        {
            Handheld.Vibrate();
        }
    }

    private void PlaySound(GameSound type)
    {
        switch (type)
        {
            case GameSound.Win: winSound.Play(); break;
            case GameSound.Lose: loseSound.Play(); break;
            case GameSound.Check: checkSound.Play(); break;
            case GameSound.Spice: spiceSound.Play(); break;
            case GameSound.Unlock: unlockSound.Play(); break;
            default: throw new System.NotSupportedException();
        }
    }

    #region SETTINGS
    private void TurnSound(bool isSoundOn)
    {
        if (isSoundOn)
        {
            mixerMasterGroup.audioMixer.SetFloat("_MasterVolume", 0);
        }
        else
        {
            mixerMasterGroup.audioMixer.SetFloat("_MasterVolume", -80);
        }
    }

    private void TurnVibration(bool isVibroOn)
    {
        vibroEnabled = isVibroOn;
    }
    #endregion
}
