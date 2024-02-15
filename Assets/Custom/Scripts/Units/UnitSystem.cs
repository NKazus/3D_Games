using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitSystem : MonoBehaviour
{
    [SerializeField] private Unit[] units;
    [SerializeField] private Vector3 offScreenPos;
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
    private System.Action<FieldCell> DestroyCallback;

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
                if (UnitCallback != null)
                {
                    UnitCallback(target);
                }
                return;
            }

            if(currentActive.GetUnitCategory() != target.GetUnitCategory() && CheckActionZone(currentActive, target))
            {
                currentActive.Attack(target);
                if (UnitCallback != null)
                {
                    UnitCallback(currentActive);
                }
                currentActive = null;
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

        if(DestroyCallback != null)
        {
            DestroyCallback(target.GetUnitCell());
        }

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
            units[i].SetDestroyCallback(DestroyUnit, offScreenPos);
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

    public List<Unit> GetActiveUnits()
    {
        return activeUnits;
    }

    public void ResetUnits()
    {
        ResetTarget();
        activeUnits.Clear();

        for (int i = 0; i < units.Length; i++)
        {
            activeUnits.Add(units[i]);
            units[i].ResetUnit();
        }
    }

    public void SetTarget(Unit target)
    {
        currentActive = target;
    }

    public void ResetTarget()
    {
        currentActive = null;
    }

    public void RefreshUnits(UnitCategory category)
    {
        for (int i = 0; i < activeUnits.Count; i++)
        {
            if(activeUnits[i].GetUnitCategory() == category)
            {
                activeUnits[i].RefreshUnit(unitActionCount);
            }
        }
    }

    public bool CheckActionZone(Unit actor1, Unit actor2)
    {
        CellIndices cell1, cell2;
        cell1 = actor1.GetUnitCell().GetIndices();
        cell2 = actor2.GetUnitCell().GetIndices();

        return (Mathf.Abs(cell1.cellX - cell2.cellX) <= 1) && (Mathf.Abs(cell1.cellZ - cell2.cellZ) <= 1);
    }

    public float CheckUnitsDistance(FieldCell actor1, FieldCell actor2)
    {
        CellIndices cell1, cell2;
        cell1 = actor1.GetIndices();
        cell2 = actor2.GetIndices();

        return Mathf.Sqrt((cell2.cellX - cell1.cellX) * (cell2.cellX - cell1.cellX)
            + (cell2.cellZ - cell1.cellZ) * (cell2.cellZ - cell1.cellZ));
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

    public void PerformAction()
    {
        List<Unit> targetUnits = new List<Unit>();
        for(int i = 0; i < activeUnits.Count; i++)
        {
            if(currentActive == activeUnits[i])
            {
                continue;
            }
            if (CheckActionZone(currentActive, activeUnits[i]))
            {
                targetUnits.Add(activeUnits[i]);
            }
        }

        currentActive.Act(targetUnits);
        if (UnitCallback != null)
        {
            UnitCallback(currentActive);
        }
        currentActive = null;
    }

    public void AddActions()
    {
        currentActive.UpdateActions();
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

    public void SetDestroyCallback(System.Action<FieldCell> callback)
    {
        DestroyCallback = callback;
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

    public void SwitchUnits(UnitCategory category, bool active)
    {
        switch (category)
        {
            case UnitCategory.Player:
                for (int i = 0; i < playerUnits.Count; i++)
                {
                    playerUnits[i].SwitchUnit(active);
                }
                break;
            case UnitCategory.Bot:
                for (int i = 0; i < botUnits.Count; i++)
                {
                    botUnits[i].SwitchUnit(active);
                }
                break;
            default: throw new System.NotSupportedException();
        }
    }
}
