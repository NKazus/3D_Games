using System;

public static class GlobalEventManager
{
    public static event Action<bool> GameStateEvent;
    public static event Action VibroEvent;
    public static event Action RewardSoundEvent;
    public static event Action<bool> VibroSettingsEvent;
    public static event Action<bool> SoundSettingsEvent;

    public static event Action<int, int, int, int> WinEvent;
    public static event Action<int, int> QueueWinEvent;


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

    public static void SetVibro(bool value)
    {
        VibroSettingsEvent?.Invoke(value);
    }

    public static void SetSound(bool value)
    {
        SoundSettingsEvent?.Invoke(value);
    }

    public static void DoWin(int value1, int value2, int value3, int value4)
    {
        WinEvent?.Invoke(value1, value2, value3, value4);
    }

    public static void DoQueueWin(int value1, int value2)
    {
        QueueWinEvent?.Invoke(value1, value2);
    }
}
