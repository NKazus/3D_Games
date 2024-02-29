using System;

public class GameEvents
{
    public event Action<bool> GameEvent;
    public event Action VibroEvent;
    public event Action<GameAudio> SoundEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action WinEvent;

    public void TriggerGame(bool activate)
    {
        GameEvent?.Invoke(activate);
    }

    public void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public void PlaySound(GameAudio type)
    {
        SoundEvent?.Invoke(type);
    }

    public void SetVibro(bool value)
    {
        VibroSettingsEvent?.Invoke(value);
    }

    public void SetSound(bool value)
    {
        SoundSettingsEvent?.Invoke(value);
    }

    public void DoWin()
    {
        WinEvent?.Invoke();
    }
}
