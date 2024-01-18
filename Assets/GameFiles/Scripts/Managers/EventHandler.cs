using System;

public class EventHandler
{
    public event Action<bool> GameModeEvent;
    public event Action VibroEvent;
    public event Action<GameSound> SoundEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action<EndGameState, int> EndGameEvent;


    public void SwitchGameMode(bool activate)
    {
        GameModeEvent?.Invoke(activate);
    }

    public void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public void PlaySound(GameSound type)
    {
        SoundEvent?.Invoke(type);
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
}
