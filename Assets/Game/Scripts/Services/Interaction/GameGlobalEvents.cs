using System;

public class GameGlobalEvents
{
    public event Action<bool> EvacuationEvent;
    public event Action<FinishCondition> GameFinishEvent;

    public event Action VibroEvent;
    public event Action RewardSoundEvent;
    public event Action StickSoundEvent;
    public event Action BreakSoundEvent;

    public event Action<bool> VibroSetupEvent;
    public event Action<bool> VolumeSetupEvent;

    public event Action<PlayerID> FinishEvent;
    public event Action<PlayerID> CollisionEvent;

    public void SwitchGame(bool activate)
    {
        EvacuationEvent?.Invoke(activate);
    }

    public void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public void PlayReward()
    {
        RewardSoundEvent?.Invoke();
    }

    public void PlayStick()
    {
        StickSoundEvent?.Invoke();
    }

    public void PlayBreak()
    {
        BreakSoundEvent?.Invoke();
    }

    public void SetVibro(bool value)
    {
        VibroSetupEvent?.Invoke(value);
    }

    public void SetVolume(bool value)
    {
        VolumeSetupEvent?.Invoke(value);
    }

    public void FinishGame(FinishCondition value)
    {
        GameFinishEvent?.Invoke(value);
    }

    public void FinishPlayer(PlayerID id)
    {
        FinishEvent?.Invoke(id);
    }

    public void CollidePlayer(PlayerID id)
    {
        CollisionEvent?.Invoke(id);
    }
}
