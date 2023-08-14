using System;

public class GlobalEventManager
{
    public event Action<bool> GameStateEvent;
    public event Action VibroEvent;
    public event Action RewardSoundEvent;
    public event Action BonusSoundEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action<int> WinEvent;
    public event Action TimeoutEvent;
    public event Action CoinEvent;

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

    public void PlayBonus()
    {
        BonusSoundEvent?.Invoke();
    }

    public void SetVibro(bool value)
    {
        VibroSettingsEvent?.Invoke(value);
    }

    public void SetSound(bool value)
    {
        SoundSettingsEvent?.Invoke(value);
    }

    public void DoWin(int value)
    {
        WinEvent?.Invoke(value);
    }

    public void TriggerTimeout()
    {
        TimeoutEvent?.Invoke();
    }

    public void CollectCoin()
    {
        CoinEvent?.Invoke();
    }
}
