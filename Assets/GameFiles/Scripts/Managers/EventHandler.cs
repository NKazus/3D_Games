using System;

public class EventHandler
{
    public event Action<bool> GameModeEvent;
    public event Action VibroEvent;
    public event Action RewardSoundEvent;
    public event Action MultSoundEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action DiceActivationEvent;
    public event Action<EndGameState, int> EndGameEvent;


    public void SwitchGameMode(bool activate)
    {
        GameModeEvent?.Invoke(activate);
    }

    public void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public void PlayReward()
    {
        RewardSoundEvent?.Invoke();
    }

    public void PlayMult()
    {
        MultSoundEvent?.Invoke();
    }

    public void SetVibro(bool value)
    {
        VibroSettingsEvent?.Invoke(value);
    }

    public void SetSound(bool value)
    {
        SoundSettingsEvent?.Invoke(value);
    }

    public void PlayEndGame(EndGameState state, int points)
    {
        EndGameEvent?.Invoke(state, points);
    }

    public void ActivateDices()
    {
        DiceActivationEvent?.Invoke();
    }
}
