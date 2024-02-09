using UnityEngine;
using Zenject;

public class ChainMode : MonoBehaviour
{
    [SerializeField] private LampSystem lampSystem;
    [SerializeField] private ActionSystem actionSystem;

    [Inject] private readonly AppEvents events;

    private void Awake()
    {
        lampSystem.Initialize(HandleLamp);
    }

    private void OnEnable()
    {
        events.GameEvent += ChangeFieldState;

        actionSystem.ResetActions();
        lampSystem.ResetLamps();
    }

    private void OnDisable()
    {
        if (IsInvoking("PlayEvent"))
        {
            CancelInvoke("PlayEvent");
        }
        if (IsInvoking("FinishTurn"))
        {
            CancelInvoke("FinishTurn");
        }

        events.GameEvent -= ChangeFieldState;
    }

    private void ChangeFieldState(bool activate)
    {
        if (activate)
        {
            actionSystem.ResetActions();
            lampSystem.ResetLamps();
            lampSystem.ActivateLamps();
            lampSystem.SwitchLamps(true);
        }
        else
        {
            lampSystem.DeactivateLamps();
        }
    }

    private void HandleLamp()
    {
        lampSystem.SwitchLamps(false);
        if (actionSystem.UseAction())
        {
            events.PlaySound(AppSound.Action);
        }

        //Invoke("FinishTurn", 0.5f);
        Invoke("PlayEvent", 0.5f);
    }

    private void PlayEvent()
    {
        lampSystem.ExtinguishRandom();

        Invoke("FinishTurn", 0.5f);
    }

    private void FinishTurn()
    {
        if (lampSystem.CheckComplete())
        {
            events.DoFinish(GameResult.Win);
            events.PlaySound(AppSound.Win);
            events.DoGame(false);
            return;
        }

        if (!actionSystem.CheckActions())
        {
            events.DoFinish(GameResult.Lose);
            events.PlaySound(AppSound.Lose);
            events.PlayVibro();
            events.DoGame(false);
            return;
        }        

        lampSystem.SwitchLamps(true);
    }
}
