using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FillMode : MonoBehaviour
{
    [SerializeField] private FillLampSystem lampSystem;
    [SerializeField] private MessageSystem messages;
    [SerializeField] private Button hideMessage;

    [Inject] private readonly AppResourceManager resources;
    [Inject] private readonly AppEvents events;

    private void Awake()
    {
        lampSystem.Initialize(HandleLamp);
        messages.Initialize();
    }

    private void OnEnable()
    {
        resources.Refresh();
        lampSystem.ResetLamps();

        messages.ResetMessages();
        CheckAvailability();
        
        hideMessage.onClick.AddListener(HideMessage);        
    }

    private void OnDisable()
    {
        hideMessage.onClick.RemoveListener(HideMessage);
        ChangeFieldState(false);

        if (IsInvoking("PlayEvent"))
        {
            CancelInvoke("PlayEvent");
        }
        if (IsInvoking("FinishTurn"))
        {
            CancelInvoke("FinishTurn");
        }
    }

    private void ChangeFieldState(bool activate)
    {
        if (activate)
        {
            lampSystem.ResetLamps();
            lampSystem.SetChainLamps();
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

        Invoke("PlayBot", 0.5f);
    }

    private void PlayBot()
    {
        lampSystem.PickRandom();

        Invoke("FinishTurn", 0.5f);
    }

    private void FinishTurn()
    {
        if (lampSystem.CheckComplete())
        {
            resources.UpdateRes(PlayerRes.Lamp, -1);
            ChangeFieldState(false);
            if (lampSystem.CalculateVictory())
            {
                resources.UpdateRes(PlayerRes.FreeAction, 1);
                events.PlaySound(AppSound.Win);
                messages.ShowResult(true);
            }
            else
            {
                events.PlayVibro();
                messages.ShowResult(false);
            }
            return;
        }

        lampSystem.SwitchLamps(true);
    }

    private void HideMessage()
    {
        CheckAvailability();
    }

    private void CheckAvailability()
    {
        if (!(resources.GetResValue(PlayerRes.Lamp) > 0))
        {
            messages.ShowLampMessage();
            return;
        }

        messages.ShowHint();
        ChangeFieldState(true);
    }
}
