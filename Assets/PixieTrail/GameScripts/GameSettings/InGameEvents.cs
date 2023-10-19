using System;

public class InGameEvents
{
    public event Action<bool> SwitchGameEvent;
    public event Action VibroEvent;
    public event Action RewardSoundEvent;
    public event Action BonusSoundEvent;
    public event Action<bool> BuffSoundEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action<int> CompleteEvent;
    public event Action PixieCollisionEvent;

    public void SwitchGame(bool activate)
    {
        SwitchGameEvent?.Invoke(activate);
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

    public void CompletePath(int value)
    {
        CompleteEvent?.Invoke(value);
    }

    public void CheckCollision()
    {
        PixieCollisionEvent?.Invoke();
    }
}
