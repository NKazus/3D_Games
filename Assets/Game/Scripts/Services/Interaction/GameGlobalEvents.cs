using System;

public class GameGlobalEvents
{
    public event Action<bool> GameStateEvent;
    public event Action WinEvent;

    public event Action VibroEvent;
    public event Action RewardSoundEvent;
    public event Action StickSoundEvent;
    public event Action BreakSoundEvent;

    public event Action<bool> VibroSetupEvent;
    public event Action<bool> VolumeSetupEvent;

    public event Action<int> ButtonPressEvent;
    public event Action StickEvent;

    public void SwitchGame(bool activate)
    {
        GameStateEvent?.Invoke(activate);
    }

    public void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public void PlayReward()
    {
        RewardSoundEvent?.Invoke();
    }

    public void PlayStick()
    {
        StickSoundEvent?.Invoke();
    }

    public void PlayBreak()
    {
        BreakSoundEvent?.Invoke();
    }

    public void SetVibro(bool value)
    {
        VibroSetupEvent?.Invoke(value);
    }

    public void SetVolume(bool value)
    {
        VolumeSetupEvent?.Invoke(value);
    }

    public void DoWin()
    {
        WinEvent?.Invoke();
    }

    public void PressButton(int buttonId)
    {
        ButtonPressEvent?.Invoke(buttonId);
    }

    public void BreakStick()
    {
        StickEvent?.Invoke();
    }
}
