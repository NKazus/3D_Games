using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource rewardSound;
    [SerializeField] private AudioSource wallSound;
    [SerializeField] private AudioSource staticWallSound;
    [SerializeField] private AudioSource gravitySound;

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
        events.GravitySoundEvent += PlayGravity;
        events.WallSoundEvent += PlayWall;
  
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
        events.GravitySoundEvent -= PlayGravity;
        events.WallSoundEvent -= PlayWall;

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

    private void PlayGravity()
    {
        gravitySound.Play();
    }

    private void PlayWall(bool isStatic)
    {
        if(isStatic)
        {
            staticWallSound.Play();
        }
        else
        {
            wallSound.Play();
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
