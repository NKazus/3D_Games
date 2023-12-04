using System;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace FitTheSize.GameServices
{
    public enum AudioEffect
    {
        Reward,
        Resource,
        Engine,
        Timer
    }
    public class GameVolume : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup mixerMasterGroup;

        [SerializeField] private AudioSource ambientMusic;
        [SerializeField] private AudioSource winSound;
        [SerializeField] private AudioSource resSound;
        [SerializeField] private AudioSource shipSound;
        [SerializeField] private AudioSource timerSound;

        private bool vibroEnabled = true;

        [Inject] private readonly GameEventHandler events;

        private void Awake()
        {
            ambientMusic.loop = true;
        }

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
                AudioEffect.Reward => winSound,
                AudioEffect.Resource => resSound,
                AudioEffect.Engine => shipSound,
                AudioEffect.Timer => timerSound,
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
