using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource rewardSound;
    [SerializeField] private AudioSource breakSound;
    [SerializeField] private AudioSource stickSound;

    private bool vibroEnabled = true;

    [Inject] private readonly InGameEvents eventManager;

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
        eventManager.StickSoundEvent += PlayStick;
        eventManager.BreakSoundEvent += PlayBreak;

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
        eventManager.StickSoundEvent -= PlayStick;
        eventManager.BreakSoundEvent -= PlayBreak;

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

    private void PlayStick()
    {
        stickSound.Play();
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
