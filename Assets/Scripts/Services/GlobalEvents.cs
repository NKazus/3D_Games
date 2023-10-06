using System;

public class GlobalEvents
{
    public event Action<bool> GameEvent;
    public event Action VibroEvent;
    public event Action RewardSoundEvent;
    public event Action GravitySoundEvent;
    public event Action<bool> WallSoundEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action<int> WinEvent;
    public event Action FailEvent;
    public event Action<int, bool> BuffEvent;
    public event Action<int> BuffMergeEvent;

    public void SwitchGravityMode(bool activate)
    {
        GameEvent?.Invoke(activate);
    }

    public void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public void PlayReward()
    {
        RewardSoundEvent?.Invoke();
    }

    public void PlayGravity()
    {
        GravitySoundEvent?.Invoke();
    }

    public void PlayWall(bool isStatic)
    {
        WallSoundEvent?.Invoke(isStatic);
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
