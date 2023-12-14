using System;

public class EventManager
{
    public event Action<bool> GameStateEvent;
    public event Action VibroEvent;
    public event Action<AudioEffect> SoundEvent;
    public event Action<bool> VibroSettingsEvent;
    public event Action<bool> SoundSettingsEvent;

    public event Action<bool> MeteorEvent;
    public event Action<bool> MeteorTriggerEvent;
    public event Action CollapseEvent;
    public event Action<int, int> ResultEvent;

    public event Action TimeOutEvent;

    public void SwitchGameState(bool activate)
    {
        GameStateEvent?.Invoke(activate);
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

    public void DoResult(int hits, int misses)
    {
        ResultEvent?.Invoke(hits, misses);
    }

    public void TriggerTimeout()
    {
        TimeOutEvent?.Invoke();
    }

    public void CalculateMeteor(bool destroyed)
    {
        MeteorEvent?.Invoke(destroyed);
    }

    public void CollapseAll()
    {
        CollapseEvent?.Invoke();
    }

    public void DoTriggerEvent(bool active)
    {
        MeteorTriggerEvent?.Invoke(active);
    }

    public void PlaySound(AudioEffect id)
    {
        SoundEvent?.Invoke(id);
    }
}
