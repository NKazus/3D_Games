using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;

    private bool vibroEnabled = true;

    [Inject] private readonly EventManager events;

    #region MONO
    private void Awake()
    {
        ambientMusic.loop = true;
    }

    private void OnEnable()
    {
        events.VibroEvent += PlayVibro;
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
        events.VibroSettingsEvent -= TurnVibration;
        events.SoundSettingsEvent -= TurnSound;
    }
    #endregion


    public void PlayVibro()
    {
        if (vibroEnabled)
        {
            Handheld.Vibrate();
        }
    }

    #region SETTINGS
    public void TurnSound(bool isSoundOn)
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

    public void TurnVibration(bool isVibroOn)
    {
        vibroEnabled = isVibroOn;
    }
    #endregion
}
