using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource rewardSound;
    [SerializeField] private AudioSource toolSound;

    private bool vibroEnabled = true;

    #region MONO
    private void Awake()
    {
        ambientMusic.loop = true;
    }

    private void OnEnable()
    {
        GlobalEventManager.VibroEvent += PlayVibro;
        GlobalEventManager.RewardSoundEvent += PlayReward;
        GlobalEventManager.BonusSoundEvent += PlayBonus;

        GlobalEventManager.VibroSettingsEvent += TurnVibration;
        GlobalEventManager.SoundSettingsEvent += TurnSound;
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
        GlobalEventManager.VibroEvent -= PlayVibro;
        GlobalEventManager.RewardSoundEvent -= PlayReward;
        GlobalEventManager.BonusSoundEvent -= PlayBonus;

        GlobalEventManager.VibroSettingsEvent -= TurnVibration;
        GlobalEventManager.SoundSettingsEvent -= TurnSound;
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
        toolSound.Play();
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
