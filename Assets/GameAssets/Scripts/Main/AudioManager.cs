using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource rewardSound;
    [SerializeField] private AudioSource museumSound;
    [SerializeField] private AudioSource marketSound;

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
        eventManager.RewardSoundEvent += PlayReward;
        eventManager.MuseumSoundEvent += PlayMuseum;
        eventManager.MarketSoundEvent += PlayMarket;

        eventManager.VibroSettingsEvent += TurnVibration;
        eventManager.SoundSettingsEvent += TurnSound;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("_SoundOn"))
        {
            TurnSound(PlayerPrefs.GetInt("_SoundOn") == 1);
            TurnVibration(PlayerPrefs.GetInt("_VibroOn") == 1);
        }
        else
        {
            TurnSound(true);
            TurnVibration(true);
            PlayerPrefs.SetInt("_SoundOn", 1);
            PlayerPrefs.SetInt("_VibroOn", 1);
        }
    }

    private void OnDisable()
    {
        eventManager.VibroEvent -= PlayVibro;
        eventManager.RewardSoundEvent -= PlayReward;
        eventManager.MuseumSoundEvent -= PlayMuseum;
        eventManager.MarketSoundEvent -= PlayMarket;

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
