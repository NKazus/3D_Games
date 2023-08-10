using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource coinSound;

    [SerializeField] private AudioSource rainSound;
    [SerializeField] private AudioSource heatSound;
    [SerializeField] private AudioSource windSound;

    [SerializeField] private AudioSource waterSound;
    [SerializeField] private AudioSource propSound;

    private bool vibroEnabled = true;

    [Inject] private readonly GlobalEventManager eventManager;

    #region MONO
    private void Awake()
    {
        ambientMusic.loop = true;
    }

    private void OnEnable()
    {
        eventManager.VibroEvent += PlayVibro;
        eventManager.CoinSoundEvent += PlayCoins;
        eventManager.WeatherSoundEvent += PlayWeather;
        eventManager.ActionSoundEvent += PlayAction;

        eventManager.VibroSettingsEvent += TurnVibration;
        eventManager.SoundSettingsEvent += TurnSound;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("_SoundOn"))
        {
            PlayerPrefs.SetInt("_SoundOn", 1);
            PlayerPrefs.SetInt("_VibroOn", 1);
        }

        TurnSound(PlayerPrefs.GetInt("_SoundOn") == 1);
        TurnVibration(PlayerPrefs.GetInt("_VibroOn") == 1);
    }

    private void OnDisable()
    {
        eventManager.VibroEvent -= PlayVibro;
        eventManager.CoinSoundEvent -= PlayCoins;
        eventManager.WeatherSoundEvent -= PlayWeather;
        eventManager.ActionSoundEvent -= PlayAction;

        eventManager.VibroSettingsEvent -= TurnVibration;
        eventManager.SoundSettingsEvent -= TurnSound;
    }
    #endregion


    private void PlayVibro()
    {
        if (vibroEnabled)
        {
            Handheld.Vibrate();
        }
    }

    private void PlayCoins()
    {
        coinSound.Play();
    }

    private void PlayWeather(int id)
    {
        switch (id)
        {
            case 1: rainSound.Play(); break;
            case 2: heatSound.Play(); break;
            default: windSound.Play(); break;
        }
    }

    private void PlayAction(int id)
    {
        switch (id)
        {
            case 1: waterSound.Play(); break;
            case 2:
            default: propSound.Play(); break;
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
