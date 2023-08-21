using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;

    private bool vibroEnabled = true;

    #region MONO
    private void Awake()
    {
        ambientMusic.loop = true;
    }

    private void OnEnable()
    {
       /* GlobalEventManager.VibroEvent += PlayVibro;
        GlobalEventManager.VibroSettingsEvent += TurnVibration;
        GlobalEventManager.SoundSettingsEvent += TurnSound;*/
    }

    private void Start()
    {
        TurnSound(PlayerPrefs.GetInt("_SoundEnabled") == 1);
        TurnVibration(PlayerPrefs.GetInt("_VibroEnabled") == 1);
    }

    private void OnDisable()
    {
        /*GlobalEventManager.VibroEvent -= PlayVibro;
        GlobalEventManager.VibroSettingsEvent -= TurnVibration;
        GlobalEventManager.SoundSettingsEvent -= TurnSound;*/
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
