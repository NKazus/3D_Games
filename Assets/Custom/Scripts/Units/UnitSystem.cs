using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitSystem : MonoBehaviour
{
    [SerializeField] private Unit[] units;

    [SerializeField] private int unitActionCount;

    private List<Unit> playerUnits = new List<Unit>();
    private List<Unit> botUnits = new List<Unit>();

    private List<Unit> activeUnits = new List<Unit>();

    private FieldCell[] playerStartPositions;
    private FieldCell[] botStartPositions;

    private GamePhase currentPhase;
    private Unit currentActive;

    private System.Action<Unit> UnitCallback;
    private System.Action SwitchCallback;
    private System.Action<bool> NoUnitCallback;

    [Inject] private readonly RandomValueGenerator generator;

    private void PickUnit(Unit target)//attack handled here
    {
        if (currentActive == null)
        {
            if (target.GetUnitCategory() == UnitCategory.Player)
            {
                currentActive = target;
                if (currentPhase != GamePhase.Switch)
                {
                    //cells switch
                    if (UnitCallback != null)
                    {
                        UnitCallback(target);
                    }
                }
            }
            return;
        }

        if (currentPhase == GamePhase.Switch)
        {
            if (target.GetUnitCategory() != UnitCategory.Player)
            {
                return;
            }

            if (target == currentActive)
            {
                currentActive = null;
                return;
            }

            SwitchUnitPositions(target, currentActive);
        }
        else
        {
            if (target == currentActive)
            {
                currentActive = null;
                return;
            }
        }
    }

    private void SwitchUnitPositions(Unit actor1, Unit actor2)
    {
        currentActive = null;

        FieldCell temp = actor1.GetUnitCell();
        actor1.PlaceUnit(actor2.GetUnitCell());
        actor2.PlaceUnit(temp);        

        if (SwitchCallback != null)
        {
            SwitchCallback();
        }
    }

    private void DestroyUnit(Unit target)
    {
        activeUnits.Remove(target);
        target.SwitchUnit(false);

        for(int i = 1; i < activeUnits.Count; i++)
        {
            if(activeUnits[i].GetUnitCategory() != activeUnits[i - 1].GetUnitCategory())//if different categories skip
            {
                return;
            }
        }

        NoUnitCallback(activeUnits[0].GetUnitCategory() == UnitCategory.Player);//check if only player remained

    }

    public void InitUnits(FieldCell[] playerPos, FieldCell[] botPos)
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i].Init();
            units[i].SetDestroyCallback(DestroyUnit);
            units[i].SetPickCallback(PickUnit);

            if (units[i].GetUnitCategory() == UnitCategory.Player)
            {
                playerUnits.Add(units[i]);
            }
            if (units[i].GetUnitCategory() == UnitCategory.Bot)
            {
                botUnits.Add(units[i]);
            }
        }

        if (playerUnits.Count != botUnits.Count || playerUnits.Count != playerPos.Length || botUnits.Count != botPos.Length)
        {
            throw new System.IndexOutOfRangeException();
        }

        playerStartPositions = playerPos;
        botStartPositions = botPos;
    }

    public void ResetUnits()
    {
        currentActive = null;
        activeUnits.Clear();

        for (int i = 0; i < units.Length; i++)
        {
            activeUnits.Add(units[i]);
            units[i].ResetUnit();
        }
    }

    public void RefreshUnits()
    {
        for (int i = 0; i < activeUnits.Count; i++)
        {
            activeUnits[i].ResetActions(unitActionCount);
        }
    }

    public void PlaceUnits()
    {
        generator.RandomizeArray(botStartPositions);
        generator.RandomizeArray(playerStartPositions);

        for(int i = 0; i < playerUnits.Count; i++)
        {
            playerUnits[i].PlaceUnit(playerStartPositions[i]);
            botUnits[i].PlaceUnit(botStartPositions[i]);
        }
    }

    public void MoveUnit(FieldCell targetCell)
    {
        currentActive.Move(targetCell);
        currentActive = null;
    }

    public void SetPhase(GamePhase type)
    {
        currentPhase = type;
    }

    public void SetCallback(System.Action<Unit> callback)
    {
        UnitCallback = callback;
    }

    public void SetSwitchCallback(System.Action callback)
    {
        SwitchCallback = callback;
    }

    public void SetUnitsCallback(System.Action<bool> callback)
    {
        NoUnitCallback = callback;
    }

    public void ActivateUnits()
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i].Activate();
        }
    }

    public void DeactivateUnits()
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i].Deactivate();
        }
    }

    public void SwitchUnits(bool active)
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i].SwitchUnit(active);
        }
    }
}
