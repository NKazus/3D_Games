using System;

public static class GlobalEventManager
{
    public static event Action<bool> GameStateEvent;
    public static event Action VibroEvent;
    public static event Action CoinSoundEvent;
    public static event Action PotionSoundEvent;
    public static event Action<bool> VibroSettingsEvent;
    public static event Action<bool> SoundSettingsEvent;

    public static event Action DiceActivationEvent;
    public static event Action<string> ScoreEvent;
    public static event Action WinEvent;


    public static void SwitchGameState(bool activate)
    {
        GameStateEvent?.Invoke(activate);
    }

    public static void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public static void PlayCoins()
    {
        CoinSoundEvent?.Invoke();
    }

    public static void PlayPotions()
    {
        PotionSoundEvent?.Invoke();
    }

    public static void SetVibro(bool value)
    {
        VibroSettingsEvent?.Invoke(value);
    }

    public static void SetSound(bool value)
    {
        SoundSettingsEvent?.Invoke(value);
    }

    public static void ChangeScore(string id)
    {
        ScoreEvent?.Invoke(id);
    }

    public static void DoWin()
    {
        WinEvent?.Invoke();
    }

    public static void ActivateDices()
    {
        DiceActivationEvent?.Invoke();
    }
}
