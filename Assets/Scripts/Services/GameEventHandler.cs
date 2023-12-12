using System;

namespace FitTheSize.GameServices
{
    public class GameEventHandler
    {
        public event Action<bool> GameStateEvent;
        public event Action VibroEvent;
        public event Action<AudioEffect> SoundEvent;
        public event Action<bool> VibroSettingsEvent;
        public event Action<bool> SoundSettingsEvent;
        public event Action<int, int, int> ResultEvent;


        public void SwitchGameState(bool activate)
        {
            GameStateEvent?.Invoke(activate);
        }

        public void PlayVibro()
        {
            VibroEvent?.Invoke();
        }

        public void SetVibro(bool value)
        {
            VibroSettingsEvent?.Invoke(value);
        }

        public void SetSound(bool value)
        {
            SoundSettingsEvent?.Invoke(value);
        }

        public void DoResult(int path, int scale, int total)
        {
            ResultEvent?.Invoke(path, scale, total);
        }

        public void PlaySound(AudioEffect id)
        {
            SoundEvent?.Invoke(id);
        }
    }
}
