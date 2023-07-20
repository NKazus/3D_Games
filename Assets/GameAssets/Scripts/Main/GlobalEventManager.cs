using System;

public class GlobalEventManager
{
    public event Action<bool> GameStateEvent;
    public event Action VibroEvent;
    public event Action RewardSoundEvent;
    public event Action MuseumSoundEvent;
    public event Action MarketSoundEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action<int> WinEvent;
    public event Action ToolRefreshEvent;

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

    public void PlayMuseum()
    {
        MuseumSoundEvent?.Invoke();
    }

    public void PlayMarket()
    {
        MarketSoundEvent?.Invoke();
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

    public void RefreshTools()
    {
        ToolRefreshEvent?.Invoke();
    }
}
