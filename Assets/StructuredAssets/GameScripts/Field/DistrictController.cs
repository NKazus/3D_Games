using UnityEngine;
using Zenject;

public class DistrictController : MonoBehaviour
{
    [SerializeField] private UnitSystem unitSystem;
    [SerializeField] private SlotSystem slotSystem;
    [SerializeField] private FinanceSystem financeSystem;
    [SerializeField] private Tools tools;

    [SerializeField] private FreeAction freeActionHandler; 

    private EventGenerator eventGenerator = new EventGenerator();

    private bool eventPlayed;

    [Inject] private readonly AppEvents events;
    [Inject] private readonly ValueGenerator valueGenerator;

    private void Awake()
    {
        slotSystem.InitSlots(HandleSlot, HandleUnit);
        tools.SetCallback(HandleToolAction);
    }

    private void OnEnable()
    {
        events.GameEvent += ChangeFieldState;

        tools.ResetTools();
        financeSystem.ResetMoney();
        slotSystem.ResetSlots();
    }

    private void OnDisable()
    {
        if (IsInvoking())
        {
            CancelInvoke("PlayEvent");
        }
        events.GameEvent -= ChangeFieldState;
        unitSystem.HideAllActive();
    }

    private void ChangeFieldState(bool activate)
    {
        if (activate)
        {
            financeSystem.ResetMoney();
            unitSystem.HideAllActive();
            slotSystem.ResetSlots();
            slotSystem.ActivateSlots();
            slotSystem.SwitchSlots(true);

            tools.ResetTools();

            eventPlayed = false;
            freeActionHandler.ResetUses();
        }
        else
        {
            slotSystem.DeactivateSlots();
        }
    }

    private void HandleSlot(Slot target)
    {
        bool isToolsActive = slotSystem.SetActiveSlot(target, true);

        if (isToolsActive)
        {
            //Debug.Log("panel show");
            tools.RefreshTools(true, target.IsEmpty() ? SubmenuType.Create : SubmenuType.Destroy);
        }
        else
        {
            //Debug.Log("panel hide");
            tools.RefreshTools(false);
        }
    }

    private void HandleToolAction(UnitType targetType)
    {
        slotSystem.SwitchSlots(false);
        events.PlaySound((targetType == UnitType.None) ? AppSound.Destroy : AppSound.Build);
        //do action to current active slot

        slotSystem.BindUnit(unitSystem.GenerateUnit(targetType));

        if (!eventPlayed && !freeActionHandler.UseFreeAction())
        {
            financeSystem.CalculateMoney(targetType);
        }

        slotSystem.DeactivateCurrent();
        tools.RefreshTools(false);
    }

    private void HandleUnit(Unit target, bool show, Vector3 targetPosition)
    {
        if (show)
        {
            unitSystem.ShowUnit(target, targetPosition, FinishUnit);
        }
        else
        {
            unitSystem.HideUnit(target, FinishUnit);
        }
    }

    private void FinishUnit()
    {
        if (slotSystem.CheckComplete() || !financeSystem.CheckMoney())
        {
            events.DoFinish(slotSystem.GetSlotsRate());
            events.PlaySound(AppSound.Result);
            events.DoGame(false);
            return;
        }

        if (!eventPlayed && GenerateEvent())
        {
            //Debug.Log("Event time");
            return;
        }

        //Debug.Log("Finish time");
        eventPlayed = false;
        slotSystem.SwitchSlots(true);
    }

    private bool GenerateEvent()
    {
        if(valueGenerator.GenerateInt(0, 10) > 4)
        {
            //Debug.Log("play event");
            events.PlaySound(AppSound.Event);
            events.PlayVibro();
            Invoke("PlayEvent", 1f);
            return true;
        }
        else
        {
            //Debug.Log("skip event");
            return false;
        }        
    }

    private void PlayEvent()
    {
        eventPlayed = true;
        Slot eventTarget = slotSystem.GetRandomSlot();
        slotSystem.SetActiveSlot(eventTarget);
        HandleToolAction(eventGenerator.GenerateAction(eventTarget));
    }
}
