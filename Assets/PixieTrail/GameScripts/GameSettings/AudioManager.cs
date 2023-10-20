using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public enum SFXType
{
    ShieldSound,
    PollenSound,
    CoinSound,
    DeliverySound,
    CollisionSound
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource shieldAS;
    [SerializeField] private AudioSource pollenAS;
    [SerializeField] private AudioSource optionAS;
    [SerializeField] private AudioSource collisionAS;
    [SerializeField] private AudioSource deliveryAS;

    private bool vibroEnabled = true;

    [Inject] private readonly InGameEvents events;

    private void OnEnable()
    {
        events.VibroEvent += PlayVibro;
        events.SFXEvent += PlaySFX;
  
        events.VibroSetEvent += SwitchVibration;
        events.VolumeSetEvent += SwitchVolume;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("_PT_VolumeEnabled"))
        {
            PlayerPrefs.SetInt("_PT_VolumeEnabled", 1);
            PlayerPrefs.SetInt("_PT_VibrationEnabled", 1);
        }

        SwitchVolume(PlayerPrefs.GetInt("_PT_VolumeEnabled") == 1);
        SwitchVibration(PlayerPrefs.GetInt("_PT_VibrationEnabled") == 1);
    }

    private void OnDisable()
    {
        events.VibroEvent -= PlayVibro;
        events.SFXEvent -= PlaySFX;

        events.VibroSetEvent -= SwitchVibration;
        events.VolumeSetEvent -= SwitchVolume;
    }


    private void PlayVibro()
    {
        if (vibroEnabled)
        {
            Handheld.Vibrate();
        }
    }

    private void PlaySFX(SFXType type)
    {
        switch (type)
        {
            case SFXType.ShieldSound: shieldAS.Play(); break;
            case SFXType.PollenSound: pollenAS.Play(); break;
            case SFXType.CoinSound: optionAS.Play(); break;
            case SFXType.DeliverySound: deliveryAS.Play(); break;
            case SFXType.CollisionSound: collisionAS.Play(); break;
            default: throw new System.NotSupportedException();
        }
    }

    private void SwitchVolume(bool isSoundOn)
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

    private void SwitchVibration(bool isVibroOn)
    {
        vibroEnabled = isVibroOn;
    }
}
