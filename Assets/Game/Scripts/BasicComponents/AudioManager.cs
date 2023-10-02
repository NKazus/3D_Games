using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource rewardSound;
    [SerializeField] private AudioSource bonusSound;
    [SerializeField] private AudioSource healSound;
    [SerializeField] private AudioSource boostSound;

    private bool vibroEnabled = true;

    [Inject] private readonly GlobalEvents events;

    #region MONO
    private void Awake()
    {
        ambientMusic.loop = true;
    }

    private void OnEnable()
    {
        events.VibroEvent += PlayVibro;
        events.RewardSoundEvent += PlayReward;
        events.BonusSoundEvent += PlayBonus;
        events.BuffSoundEvent += PlayBuff;
  
        events.VibroSettingsEvent += TurnVibration;
        events.SoundSettingsEvent += TurnSound;
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
        events.VibroEvent -= PlayVibro;
        events.RewardSoundEvent -= PlayReward;
        events.BonusSoundEvent -= PlayBonus;
        events.BuffSoundEvent -= PlayBuff;

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

    private void PlayReward()
    {
        rewardSound.Play();
    }

    private void PlayBonus()
    {
        bonusSound.Play();
    }

    private void PlayBuff(bool speed)
    {
        if (speed)
        {
            boostSound.Play();
        }
        else
        {
            healSound.Play();
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
