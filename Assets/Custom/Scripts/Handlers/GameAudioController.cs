using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public enum GameAudio
{
    Victory,
    Loss,
    Bounce
}

public class GameAudioController : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource victorySound;
    [SerializeField] private AudioSource lossSound;
    [SerializeField] private AudioSource bounceSound;

    private bool vibroEnabled = true;

    [Inject] private readonly GameEvents events;

    #region MONO
    private void OnEnable()
    {
        events.VibroEvent += PlayVibro;
        events.SoundEvent += PlayAudio;

        events.VibroSettingsEvent += TurnVibration;
        events.SoundSettingsEvent += TurnSound;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("AppSettings_Sound"))
        {
            PlayerPrefs.SetInt("AppSettings_Sound", 1);
            PlayerPrefs.SetInt("AppSettings_Vibro", 1);
        }
        TurnSound(PlayerPrefs.GetInt("AppSettings_Sound") == 1);
        TurnVibration(PlayerPrefs.GetInt("AppSettings_Vibro") == 1);
    }

    private void OnDisable()
    {
        events.VibroEvent -= PlayVibro;
        events.SoundEvent -= PlayAudio;

        events.VibroSettingsEvent -= TurnVibration;
        events.SoundSettingsEvent -= TurnSound;
    }
    #endregion

    private void PlayVibro()
    {
        if (vibroEnabled)
        {
            Handheld.Vibrate();
        }
    }

    private void PlayAudio(GameAudio type)
    {
        AudioSource target = type switch
        {
            GameAudio.Victory => victorySound,
            GameAudio.Loss => lossSound,
            GameAudio.Bounce => bounceSound,
            _ => throw new System.NotSupportedException()
        };
        target.Play();
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
