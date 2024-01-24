using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AppAudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource rewardSound;
    [SerializeField] private AudioSource museumSound;
    [SerializeField] private AudioSource marketSound;

    private bool vibroEnabled = true;

    [Inject] private readonly AppEvents appEvents;

    #region MONO
    private void Awake()
    {
        ambientMusic.loop = true;
    }

    private void OnEnable()
    {
        appEvents.VibroEvent += PlayVibro;
        appEvents.SoundEvent += PlayReward;
        appEvents.MuseumSoundEvent += PlayMuseum;
        appEvents.MarketSoundEvent += PlayMarket;

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
        appEvents.SoundEvent -= PlayReward;
        appEvents.MuseumSoundEvent -= PlayMuseum;
        appEvents.MarketSoundEvent -= PlayMarket;

        appEvents.VibroSettingsEvent -= TurnVibration;
        appEvents.SoundSettingsEvent -= TurnSound;
    }
    #endregion


    private void PlayVibro()
    {
        if (vibroEnabled)
        {
            Handheld.Vibrate();
        }
    }

    private void PlayReward()
    {
        rewardSound.Play();
    }

    private void PlayMuseum()
    {
        museumSound.Play();
    }

    private void PlayMarket()
    {
        marketSound.Play();
    }

    #region SETTINGS
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
    #endregion
}
