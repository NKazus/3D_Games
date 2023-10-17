using System;

public class InGameEvents
{
    public event Action<bool> GameStateEvent;
    public event Action VibroEvent;
    public event Action RewardSoundEvent;
    public event Action BonusSoundEvent;
    public event Action<bool> BuffSoundEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action<int> WinEvent;
    public event Action FailEvent;
    public event Action<int, bool> BuffEvent;
    public event Action<int> BuffMergeEvent;

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

    public void PlayBuff(bool isSpeed)
    {
        BuffSoundEvent?.Invoke(isSpeed);
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

    public void TriggerFail()
    {
        FailEvent?.Invoke();
    }

    public void CalculateBuff(int id, bool active)
    {
        BuffEvent?.Invoke(id, active);
    }

    public void RemoveBuff(int id)
    {
        BuffMergeEvent?.Invoke(id);
    }
}
