using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource rewardSound;
    [SerializeField] private AudioSource breakSound;
    [SerializeField] private AudioSource stickSound;

    private bool vibroEnabled = true;

    [Inject] private readonly GameGlobalEvents eventManager;

    #region MONO
    private void Awake()
    {
        ambientMusic.loop = true;

        if (!PlayerPrefs.HasKey("_GameAudio_Volume"))
        {
            PlayerPrefs.SetInt("_GameAudio_Volume", 1);
            PlayerPrefs.SetInt("_GameAudio_Vibro", 1);
        }
    }

    private void OnEnable()
    {
        eventManager.VibroEvent += PlayVibro;
        eventManager.RewardSoundEvent += PlayReward;
        eventManager.StickSoundEvent += PlayStick;
        eventManager.BreakSoundEvent += PlayBreak;

        eventManager.VibroSetupEvent += SwitchAppVibro;
        eventManager.VolumeSetupEvent += SwitchAppVolume;
    }

    private void Start()
    {
        SwitchAppVolume(PlayerPrefs.GetInt("_GameAudio_Volume") == 1);
        SwitchAppVibro(PlayerPrefs.GetInt("_GameAudio_Vibro") == 1);
    }

    private void OnDisable()
    {
        eventManager.VibroEvent -= PlayVibro;
        eventManager.RewardSoundEvent -= PlayReward;
        eventManager.StickSoundEvent -= PlayStick;
        eventManager.BreakSoundEvent -= PlayBreak;

        eventManager.VibroSetupEvent -= SwitchAppVibro;
        eventManager.VolumeSetupEvent -= SwitchAppVolume;
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
    private void SwitchAppVolume(bool isVolumeOn)
    {
        if (isVolumeOn)
        {
            mixerMasterGroup.audioMixer.SetFloat("_MasterVolume", 0);
        }
        else
        {
            mixerMasterGroup.audioMixer.SetFloat("_MasterVolume", -80);
        }
    }

    private void SwitchAppVibro(bool isVibroOn)
    {
        vibroEnabled = isVibroOn;
    }
    #endregion
}
