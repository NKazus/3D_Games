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
        Debug.Log("Lamp click");
        lampSystem.SwitchLamps(false);
        actionSystem.UseAction();

        //Invoke("FinishTurn", 0.5f);
        Invoke("PlayEvent", 0.5f);
    }

    private void PlayEvent()
    {
        //Debug.Log("play event");
        lampSystem.ExtinguishRandom();

        Invoke("FinishTurn", 0.5f);
    }

    private void FinishTurn()
    {
        Debug.Log("random lamp pick");
        if (lampSystem.CheckComplete())
        {
            events.DoFinish(GameResult.Win);
            Debug.Log("win");
            events.DoGame(false);
            return;
        }

        if (!actionSystem.CheckActions())
        {
            events.DoFinish(GameResult.Lose);
            Debug.Log("lose");
            events.DoGame(false);
            return;
        }
        

        lampSystem.SwitchLamps(true);
    }
}
