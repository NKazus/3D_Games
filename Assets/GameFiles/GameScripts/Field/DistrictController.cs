using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DistrictController : MonoBehaviour
{
    [SerializeField] private UnitSystem unitSystem;
    [SerializeField] private SlotSystem slotSystem;
    [SerializeField] private FinanceSystem financeSystem;
    [SerializeField] private float startFinances;
    [SerializeField] private float completeFieldMultiplyer;
    [SerializeField] private Tools tools;

    [Inject] private readonly AppResourceManager resources;
    [Inject] private readonly AppEvents events;
    [Inject] private readonly ValueGenerator randomGenerator;

    private void Awake()
    {
        slotSystem.InitSlots(HandleSlot);
        tools.SetCallback(HandleToolAction);
    }

    private void OnEnable()
    {
        events.GameEvent += ChangeFieldState;

        tools.ResetTools();
    }

    private void OnDisable()
    {
        events.GameEvent -= ChangeFieldState;
    }

    private void ChangeFieldState(bool activate)
    {
        if (activate)
        {
            unitSystem.HideAllActive();
            slotSystem.ResetSlots();
            slotSystem.ActivateSlots();
            slotSystem.SwitchSlots(true);

            tools.ResetTools();
        }
        else
        {
            slotSystem.DeactivateSlots();
        }
    }

    private void HandleSlot(Slot target)
    {
        bool isToolsActive = slotSystem.SetActiveSlot(target);

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

    private void HandleToolAction(UnitType target)
    {
        slotSystem.SwitchSlots(false);
        //do action to current active slot
        if(target == UnitType.None)
        {
            slotSystem.BindUnit(null);
        }

        //get unit from system

        
    }

    private void HandleUnit(Unit target, bool show, Vector3 targetPosition)
    {
        //do smth with unit from slot

        //unitSystem
    }

    private void FinishUnit()
    {
        //generate event or not
        slotSystem.SwitchSlots(true);
    }

}
