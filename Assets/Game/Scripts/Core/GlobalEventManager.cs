using System;

public static class GlobalEventManager
{
    public static event Action<bool> GameStateEvent;
    public static event Action VibroEvent;
    public static event Action RewardSoundEvent;
    public static event Action BonusSoundEvent;
    public static event Action<bool> VibroSettingsEvent;
    public static event Action<bool> SoundSettingsEvent;

    public static event Action<int> WinEvent;
    public static event Action ToolRefreshEvent;

    public static void SwitchGameState(bool activate)
    {
        GameStateEvent?.Invoke(activate);
    }

    public static void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public static void PlayReward()
    {
        RewardSoundEvent?.Invoke();
    }

    public static void PlayBonus()
    {
        BonusSoundEvent?.Invoke();
    }

    public static void SetVibro(bool value)
    {
        VibroSettingsEvent?.Invoke(value);
    }

    public static void SetSound(bool value)
    {
        SoundSettingsEvent?.Invoke(value);
    }

    public static void DoWin(int value)
    {
        WinEvent?.Invoke(value);
    }

    public static void RefreshTools()
    {
        ToolRefreshEvent?.Invoke();
    }
}
