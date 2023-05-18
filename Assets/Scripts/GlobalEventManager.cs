using System;
using UnityEngine;

public static class GlobalEventManager
{
    public static event Action<bool> GameStateEvent;
    public static event Action<bool> BonusStateEvent;
    public static event Action HidePlatformsEvent;
    public static event Action VibroEvent;
    public static event Action<bool> VibroSettingsEvent;
    public static event Action<bool> SoundSettingsEvent;

    public static event Action<Vector3> MovePlayerEvent;
    public static event Action PushPlayerEvent;
    public static event Action<string> ScoreEvent;
    public static event Action<bool> BonusScoreEvent;
    public static event Action<float> RotationSpeedEvent;
    public static event Action WinEvent;


    public static void SwitchGameState(bool activate)
    {
        GameStateEvent?.Invoke(activate);
    }

    public static void SwitchBonusState(bool activate)
    {
        BonusStateEvent?.Invoke(activate);
    }

    public static void PlayVibro()
    {
        VibroEvent?.Invoke();
    }

    public static void SetVibro(bool value)
    {
        VibroSettingsEvent?.Invoke(value);
    }

    public static void SetSound(bool value)
    {
        SoundSettingsEvent?.Invoke(value);
    }

    public static void HidePlatforms()
    {
        HidePlatformsEvent?.Invoke();
    }

    public static void PushPlayer()
    {
        PushPlayerEvent?.Invoke();
    }

    public static void MovePlayer(Vector3 position)
    {
        MovePlayerEvent?.Invoke(position);
    }

    public static void ChangeScore(string id)
    {
        ScoreEvent?.Invoke(id);
    }

    public static void ChangeBonusScore(bool isCollected)
    {
        BonusScoreEvent?.Invoke(isCollected);
    }

    public static void UpdateRotation(float value)
    {
        RotationSpeedEvent?.Invoke(value);
    }

    public static void DoWin()
    {
        WinEvent?.Invoke();
    }
}
