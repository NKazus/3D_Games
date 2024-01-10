using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class GameAudio : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource rewardSound;
    [SerializeField] private AudioSource multSound;

    private bool vibroEnabled = true;

    [Inject] private readonly EventHandler events;

    private void OnEnable()
    {
        events.VibroEvent += PlayVibro;
        events.RewardSoundEvent += PlayReward;
        events.MultSoundEvent += PlayMult;

        events.VibroSettingsEvent += TurnVibration;
        events.SoundSettingsEvent += TurnSound;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("_DWL_SoundEnabled"))
        {
            PlayerPrefs.SetInt("_DWL_SoundEnabled", 1);
            PlayerPrefs.SetInt("_DWL_VibroEnabled", 1);
        }

        TurnSound(PlayerPrefs.GetInt("_DWL_SoundEnabled") == 1);
        TurnVibration(PlayerPrefs.GetInt("_DWL_VibroEnabled") == 1);
    }

    private void OnDisable()
    {
        events.VibroEvent -= PlayVibro;
        events.RewardSoundEvent -= PlayReward;
        events.MultSoundEvent -= PlayMult;

        events.VibroSettingsEvent -= TurnVibration;
        events.SoundSettingsEvent -= TurnSound;
    }


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
