using System;

public class AppEvents
{
    public event Action<bool> GameEvent;
    public event Action VibroEvent;
    public event Action<AppSound> SoundEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action<GameResult> FinishEvent;
    public event Action ToolRefreshEvent;

    public void DoGame(bool activate)
    {
        GameEvent?.Invoke(activate);
    }

    public void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public void PlaySound(AppSound type)
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

    public void DoFinish(GameResult type)
    {
        FinishEvent?.Invoke(type);
    }

    public void RefreshTools()
    {
        ToolRefreshEvent?.Invoke();
    }
}
