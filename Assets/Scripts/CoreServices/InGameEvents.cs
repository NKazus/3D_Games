using System;

public class InGameEvents
{
    public event Action<bool> GameStateEvent;
    public event Action WinEvent;

    public event Action VibroEvent;
    public event Action ActiveSoundEvent;
    public event Action AlertSoundEvent;
    public event Action ScanSoundEvent;
    public event Action BreakSoundEvent;

    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;


    public void SwitchGameState(bool activate)
    {
        GameStateEvent?.Invoke(activate);
    }

    public void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public void PlayActive()
    {
        ActiveSoundEvent?.Invoke();
    }

    public void PlayScanner()
    {
        ScanSoundEvent?.Invoke();
    }

    public void PlayBreak()
    {
        BreakSoundEvent?.Invoke();
    }

    public void PlayAlert()
    {
        AlertSoundEvent?.Invoke();
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
