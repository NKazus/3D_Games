using System;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace FitTheSize.GameServices
{
    public enum AudioEffect
    {
        Finish,
        HighScore,
        Setup,
        ForceScale
    }
    public class GameVolume : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup mixerMasterGroup;

        [SerializeField] private AudioSource finishSound;
        [SerializeField] private AudioSource highScoreSound;
        [SerializeField] private AudioSource buttonSound;
        [SerializeField] private AudioSource scaleSound;

        private bool vibroEnabled = true;

        [Inject] private readonly GameEventHandler events;

        private void OnEnable()
        {
            events.VibroEvent += PlayVibro;
            events.SoundEvent += PlaySound;
            events.VibroSettingsEvent += SwitchVibration;
            events.SoundSettingsEvent += SwitchSound;
        }

        private void Start()
        {
            if (!PlayerPrefs.HasKey("SETTINGS_MUSIC"))
            {
                PlayerPrefs.SetInt("SETTINGS_MUSIC", 1);
                PlayerPrefs.SetInt("SETTINGS_VIBRATION", 1);
            }
            SwitchSound(PlayerPrefs.GetInt("SETTINGS_MUSIC") == 1);
            SwitchVibration(PlayerPrefs.GetInt("SETTINGS_VIBRATION") == 1);
        }

        private void OnDisable()
        {
            events.VibroEvent -= PlayVibro;
            events.SoundEvent -= PlaySound;
            events.VibroSettingsEvent -= SwitchVibration;
            events.SoundSettingsEvent -= SwitchSound;
        }

        public void PlayVibro()
        {
            if (vibroEnabled)
            {
                Handheld.Vibrate();
            }
        }

        public void PlaySound(AudioEffect id)
        {
            AudioSource source;
            source = id switch
            {
                AudioEffect.Finish => finishSound,
                AudioEffect.HighScore => highScoreSound,
                AudioEffect.Setup => buttonSound,
                AudioEffect.ForceScale => scaleSound,
                _ => throw new NotSupportedException()
            };

            source.Play();
        }

        public void SwitchSound(bool isSoundOn)
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

        public void SwitchVibration(bool isVibroOn)
        {
            vibroEnabled = isVibroOn;
        }
    }
}
