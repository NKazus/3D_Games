using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public enum AppSound
{
    Win,
    Lose,
    Action
}

public class AppAudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource actionSound;
    [SerializeField] private AudioSource loseSound;

    private bool vibroEnabled = true;

    [Inject] private readonly AppEvents appEvents;

    private void OnEnable()
    {
        appEvents.VibroEvent += PlayVibro;
        appEvents.SoundEvent += PlaySound;

        appEvents.VibroSettingsEvent += TurnVibration;
        appEvents.SoundSettingsEvent += TurnSound;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("App_Sound_On"))
        {
            TurnSound(PlayerPrefs.GetInt("App_Sound_On") == 1);
            TurnVibration(PlayerPrefs.GetInt("App_Vibro_On") == 1);
        }
        else
        {
            TurnSound(true);
            TurnVibration(true);
            PlayerPrefs.SetInt("App_Sound_On", 1);
            PlayerPrefs.SetInt("App_Vibro_On", 1);
        }
    }

    private void OnDisable()
    {
        appEvents.VibroEvent -= PlayVibro;
        appEvents.SoundEvent -= PlaySound;

        appEvents.VibroSettingsEvent -= TurnVibration;
        appEvents.SoundSettingsEvent -= TurnSound;
    }

    private void PlayVibro()
    {
        if (vibroEnabled)
        {
            Handheld.Vibrate();
        }
    }

    private void PlaySound(AppSound type)
    {
        AudioSource target = type switch
        {
            AppSound.Win => winSound,
            AppSound.Action => actionSound,
            AppSound.Lose => loseSound,
            _ => throw new System.NotSupportedException()
        };
        target.Play();
    }

    private void TurnSound(bool isSoundOn)
    {
        if (isSoundOn)
        {
            mixerMasterGroup.audioMixer.SetFloat("_MainVolume", 0);
        }
        else
        {
            mixerMasterGroup.audioMixer.SetFloat("_MainVolume", -80);
        }
    }

    private void TurnVibration(bool isVibroOn)
    {
        vibroEnabled = isVibroOn;
    }
}
