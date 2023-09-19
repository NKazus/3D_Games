using System;

public class EventManager
{
    public event Action<bool> GameStateEvent;
    public event Action VibroEvent;
    public event Action RewardSoundEvent;
    public event Action ChargeSoundEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action DiceActivationEvent;
    public event Action WinEvent;

    public void SwitchGameState(bool activate)
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

    public void PlayCharge()
    {
        ChargeSoundEvent?.Invoke();
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

    public void ActivateDices()
    {
        DiceActivationEvent?.Invoke();
    }
}
