using System;

public class GlobalEventManager
{
    public event Action<bool> GameStateEvent;
    public event Action VibroEvent;
    public event Action CoinSoundEvent;
    public event Action PotionSoundEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action<Reagent> ReagentEvent;
    public event Action<string> ScoreEvent;
    public event Action<int> WinEvent;


    public void SwitchGameState(bool activate)
    {
        GameStateEvent?.Invoke(activate);
    }

    public void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public void PlayCoins()
    {
        CoinSoundEvent?.Invoke();
    }

    public void PlayPotions()
    {
        PotionSoundEvent?.Invoke();
    }

    public void SetVibro(bool value)
    {
        VibroSettingsEvent?.Invoke(value);
    }

    public void SetSound(bool value)
    {
        SoundSettingsEvent?.Invoke(value);
    }

    public void UseReagent(Reagent value)
    {
        ReagentEvent?.Invoke(value);
    }

    public void ChangeScore(string id)
    {
        ScoreEvent?.Invoke(id);
    }

    public void DoWin(int extra)
    {
        WinEvent?.Invoke(extra);
    }
}
