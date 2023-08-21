using System;
using UnityEngine;

public class EventManager
{
    public event Action<bool> GameStateEvent;
    public event Action<bool> BonusStateEvent;
    public event Action HidePlatformsEvent;
    public event Action VibroEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action<Vector3> MovePlayerEvent;
    public event Action PushPlayerEvent;
    public event Action<string> ScoreEvent;
    public event Action<bool> BonusScoreEvent;
    public event Action<float> RotationSpeedEvent;
    public event Action WinEvent;


    public void SwitchGameState(bool activate)
    {
        GameStateEvent?.Invoke(activate);
    }

    public void SwitchBonusState(bool activate)
    {
        BonusStateEvent?.Invoke(activate);
    }

    public void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public void SetVibro(bool value)
    {
        VibroSettingsEvent?.Invoke(value);
    }

    public void SetSound(bool value)
    {
        SoundSettingsEvent?.Invoke(value);
    }

    public void HidePlatforms()
    {
        HidePlatformsEvent?.Invoke();
    }

    public void PushPlayer()
    {
        PushPlayerEvent?.Invoke();
    }

    public void MovePlayer(Vector3 position)
    {
        MovePlayerEvent?.Invoke(position);
    }

    public void ChangeScore(string id)
    {
        ScoreEvent?.Invoke(id);
    }

    public void ChangeBonusScore(bool isCollected)
    {
        BonusScoreEvent?.Invoke(isCollected);
    }

    public void UpdateRotation(float value)
    {
        RotationSpeedEvent?.Invoke(value);
    }

    public void DoWin()
    {
        WinEvent?.Invoke();
    }
}
