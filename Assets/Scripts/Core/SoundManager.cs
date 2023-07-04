using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource rewardSound;
    [SerializeField] private AudioSource multSound;

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
        GlobalEventManager.MultSoundEvent += PlayMult;

        GlobalEventManager.VibroSettingsEvent += TurnVibration;
        GlobalEventManager.SoundSettingsEvent += TurnSound;
    }

    private void Start()
    {
        TurnSound(PlayerPrefs.GetInt("_SoundEnabled") == 1);
        TurnVibration(PlayerPrefs.GetInt("_VibroEnabled") == 1);
    }

    private void OnDisable()
    {
        GlobalEventManager.VibroEvent -= PlayVibro;
        GlobalEventManager.RewardSoundEvent -= PlayReward;
        GlobalEventManager.MultSoundEvent -= PlayMult;

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

    private void PlayMult()
    {
        multSound.Play();
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
