using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource rewardSound;
    [SerializeField] private AudioSource chargeSound;
    [SerializeField] private AudioSource pickSound;

    private bool vibroEnabled = true;

    [Inject] private readonly EventManager eventManager;

    #region MONO
    private void Awake()
    {
        ambientMusic.loop = true;
    }

    private void OnEnable()
    {
        eventManager.VibroEvent += PlayVibro;
        eventManager.RewardSoundEvent += PlayReward;
        eventManager.ChargeSoundEvent += PlayCharge;
        eventManager.PickSoundEvent += PlayPick;

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
        eventManager.RewardSoundEvent -= PlayReward;
        eventManager.ChargeSoundEvent -= PlayCharge;
        eventManager.PickSoundEvent -= PlayPick;

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

    private void PlayCharge()
    {
        chargeSound.Play();
    }

    private void PlayPick()
    {
        pickSound.Play();
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
