using UnityEngine;
using UnityEngine.Audio;
using Zenject;
using MEGame.Interactions;

public enum AudioType
{
    Finish,
    Fail,
    Points
}

public class VolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource finishAudio;
    [SerializeField] private AudioSource failAudio;
    [SerializeField] private AudioSource pointsAudio;

    private bool vibroEnabled = true;

    [Inject] private readonly GameGlobalEvents eventManager;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("BOOL_VolumeValue"))
        {
            PlayerPrefs.SetInt("BOOL_VolumeValue", 1);
            PlayerPrefs.SetInt("BOOL_VibroValue", 1);
        }
    }

    private void OnEnable()
    {
        eventManager.VibroEvent += PlayVibro;
        eventManager.AudioEvent += PlayAudioSource;

        eventManager.VibroSetupEvent += SwitchAppVibro;
        eventManager.VolumeSetupEvent += SwitchAppVolume;
    }

    private void Start()
    {
        SwitchAppVolume(PlayerPrefs.GetInt("BOOL_VolumeValue") == 1);
        SwitchAppVibro(PlayerPrefs.GetInt("BOOL_VibroValue") == 1);
    }

    private void OnDisable()
    {
        eventManager.VibroEvent -= PlayVibro;
        eventManager.AudioEvent -= PlayAudioSource;

        eventManager.VibroSetupEvent -= SwitchAppVibro;
        eventManager.VolumeSetupEvent -= SwitchAppVolume;
    }

    private void PlayVibro()
    {
        if (vibroEnabled)
        {
            Handheld.Vibrate();
        }
    }

    private void PlayAudioSource(AudioType type)
    {
        AudioSource target;
        switch (type)
        {
            case AudioType.Finish: target = finishAudio; break;
            case AudioType.Fail: target = failAudio; break;
            case AudioType.Points: target = pointsAudio; break;
            default: throw new System.NotSupportedException();
        }
        target.Play();
    }

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
}
