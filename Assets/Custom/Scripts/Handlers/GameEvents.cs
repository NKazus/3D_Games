using System;

public class GameEvents
{
    public event Action<bool> GameEvent;
    public event Action VibroEvent;
    public event Action<GameAudio> SoundEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action<int> WinEvent;
    public event Action<CellIndices> MovePlayerEvent;
    public event Action LocksUpdateEvent;
    public event Action CoinEvent;

    public void TriggerGame(bool activate)
    {
        GameEvent?.Invoke(activate);
    }

    public void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public void PlaySound(GameAudio type)
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

    public void DoWin(int value)
    {
        WinEvent?.Invoke(value);
    }

    public void MovePlayer(CellIndices cell)
    {
        MovePlayerEvent?.Invoke(cell);
    }

    public void UpdateLocks()
    {
        LocksUpdateEvent?.Invoke();
    }

    public void CollectCoin()
    {
        CoinEvent?.Invoke();
    }
}
