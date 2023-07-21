using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource rewardSound;
    [SerializeField] private AudioSource bonusSound;

    private bool vibroEnabled = true;

    [Inject] private readonly EventManager eventManager;

    #region MONO
    private void Awake()
    {
        ambientMusic.loop = true;

        if (!PlayerPrefs.HasKey("_GameSound"))
        {
            PlayerPrefs.SetInt("_GameSound", 1);
            PlayerPrefs.SetInt("_GameVibro", 1);
        }
    }

    private void OnEnable()
    {
        eventManager.VibroEvent += PlayVibro;
        eventManager.RewardSoundEvent += PlayReward;
        eventManager.BonusSoundEvent += PlayBonus;

        eventManager.VibroSettingsEvent += TurnVibration;
        eventManager.SoundSettingsEvent += TurnSound;
    }

    private void Start()
    {
        TurnSound(PlayerPrefs.GetInt("_GameSound") == 1);
        TurnVibration(PlayerPrefs.GetInt("_GameVibro") == 1);
    }

    private void OnDisable()
    {
        eventManager.VibroEvent -= PlayVibro;
        eventManager.RewardSoundEvent -= PlayReward;
        eventManager.BonusSoundEvent -= PlayBonus;

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

    private void PlayBonus()
    {
        bonusSound.Play();
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
