using System;

public class InGameEvents
{
    public event Action<bool> SwitchGameEvent;
    public event Action VibroEvent;
    public event Action<SFXType> SFXEvent;

    public event Action<bool> VibroSetEvent;
    public event Action<bool> VolumeSetEvent;

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

    public void PlaySFX(SFXType type)
    {
        SFXEvent?.Invoke(type);
    }

    public void SetVibro(bool value)
    {
        VibroSetEvent?.Invoke(value);
    }

    public void SetVolume(bool value)
    {
        VolumeSetEvent?.Invoke(value);
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
