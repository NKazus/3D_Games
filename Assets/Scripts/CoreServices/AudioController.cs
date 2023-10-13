using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource activeSound;
    [SerializeField] private AudioSource alertSound;
    [SerializeField] private AudioSource breakSound;
    [SerializeField] private AudioSource scannerSound;

    private bool vibroEnabled = true;

    [Inject] private readonly InGameEvents gameEvents;

    #region MONO
    private void Awake()
    {
        backgroundMusic.loop = true;

        if (!PlayerPrefs.HasKey("_InGameVolume"))
        {
            PlayerPrefs.SetInt("_InGameVolume", 1);
            PlayerPrefs.SetInt("_InGameVibro", 1);
        }
    }

    private void OnEnable()
    {
        gameEvents.VibroEvent += PlayVibro;
        gameEvents.ActiveSoundEvent += PlayActivePhase;
        gameEvents.ScanSoundEvent += PlayScanner;
        gameEvents.AlertSoundEvent += PlayAlert;
        gameEvents.BreakSoundEvent += PlayBreak;

        gameEvents.VibroSettingsEvent += TurnVibration;
        gameEvents.SoundSettingsEvent += TurnSound;
    }

    private void Start()
    {
        TurnSound(PlayerPrefs.GetInt("_InGameVolume") == 1);
        TurnVibration(PlayerPrefs.GetInt("_InGameVibro") == 1);
    }

    private void OnDisable()
    {
        gameEvents.VibroEvent -= PlayVibro;
        gameEvents.ActiveSoundEvent -= PlayActivePhase;
        gameEvents.ScanSoundEvent -= PlayScanner;
        gameEvents.BreakSoundEvent -= PlayBreak;
        gameEvents.AlertSoundEvent -= PlayAlert;

        gameEvents.VibroSettingsEvent -= TurnVibration;
        gameEvents.SoundSettingsEvent -= TurnSound;
    }
    #endregion


    private void PlayVibro()
    {
        if (vibroEnabled)
        {
            Handheld.Vibrate();
        }
    }

    private void PlayActivePhase()
    {
        activeSound.Play();
    }

    private void PlayAlert()
    {
        alertSound.Play();
    }

    private void PlayScanner()
    {
        scannerSound.Play();
    }

    private void PlayBreak()
    {
        breakSound.Play();
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
