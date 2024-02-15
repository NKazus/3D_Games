using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bot : MonoBehaviour
{
    private UnitSystem unitSystem;
    private FieldCellSystem cellSystem;

    private List<Unit> botUnits = new List<Unit>();
    private List<Unit> playerUnits = new List<Unit>();

    private IEnumerator botCoroutine;
    private System.Action BotCallback;

    [Inject] private readonly RandomValueGenerator generator;

    private void InitTurn()
    {
        List<Unit> currentUnits = unitSystem.GetActiveUnits();
        botUnits.Clear();
        playerUnits.Clear();

        for(int i = 0; i < currentUnits.Count; i++)
        {
            if(currentUnits[i].GetUnitCategory() == UnitCategory.Bot)
            {
                botUnits.Add(currentUnits[i]);
            }
            else
            {
                playerUnits.Add(currentUnits[i]);
            }
        }

        botUnits.Sort((a, b) => b.GetUnitType().CompareTo(a.GetUnitType()));
    }

    private IEnumerator ActivateUnits()
    {
        for(int i = 0; i < botUnits.Count; i++)
        {
            DoUnit(botUnits[i]);
            yield return new WaitForSeconds(1f);
        }
        if(BotCallback != null)
        {
            BotCallback();
        }
    }

    private void DoUnit(Unit target)
    {
        Debug.Log($"!!!!! UNIT TYPE: {target.GetUnitType()}");
        switch (target.GetUnitType())
        {
            case UnitType.Buff:
            case UnitType.Defence: DoSupport(target); break;
            case UnitType.Attack: DoAttack(target); break;
            default: throw new System.NotSupportedException();
        }
    }

    private void DoAttack(Unit target)
    {
        List<Unit> attackTargets = new List<Unit>();
        float minDistance = 100f;
        float currentDistance;
        Unit closestUnit = null;

        for (int i = 0; i < playerUnits.Count; i++)
        {
            if (!playerUnits[i].IsUnitEnabled())
            {
                continue;
            }

            if (unitSystem.CheckActionZone(target, playerUnits[i]))
            {
                attackTargets.Add(playerUnits[i]);
                continue;
            }

            currentDistance = unitSystem.CheckUnitsDistance(target.GetUnitCell(), playerUnits[i].GetUnitCell());
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                closestUnit = playerUnits[i];
            }
        }

        if(attackTargets.Count > 0)
        {
            //Debug.Log("Targets spotted - Attack");
            target.Act(attackTargets);
            return;
        }

        //Debug.Log("No targets - Move");
        MoveToTarget(target, closestUnit);
    }

    private void DoSupport(Unit target)
    {
        List<Unit> enemyUnits = new List<Unit>();
        List<Unit> allyUnits = new List<Unit>();
        Unit closestEnemy = null;
        Unit closestAlly = null;

        float currentDistance;
        float minEnemyDistance = 100f;
        float minAllyDistance = 100f;

        for (int i = 0; i < playerUnits.Count; i++)
        {
            if (!playerUnits[i].IsUnitEnabled())
            {
                continue;
            }

            if (unitSystem.CheckActionZone(target, playerUnits[i]))
            {
                enemyUnits.Add(playerUnits[i]);
                continue;
            }

            currentDistance = unitSystem.CheckUnitsDistance(target.GetUnitCell(), playerUnits[i].GetUnitCell());
            if (currentDistance < minEnemyDistance)
            {
                minEnemyDistance = currentDistance;
                closestEnemy = playerUnits[i];
            }
        }

        for (int i = 0; i < botUnits.Count; i++)
        {
            if(botUnits[i] == target)
            {
                continue;
            }

            if (unitSystem.CheckActionZone(target, botUnits[i]))
            {
                allyUnits.Add(botUnits[i]);
                continue;
            }

            currentDistance = unitSystem.CheckUnitsDistance(target.GetUnitCell(), botUnits[i].GetUnitCell());
            if (currentDistance < minAllyDistance)
            {
                minAllyDistance = currentDistance;
                closestAlly = botUnits[i];
            }
        }

        bool anyAllyNearby = allyUnits.Count > 0;
        bool anyEnemyNearby = enemyUnits.Count > 0;

        if (botUnits.Count <= 1)//no allies remained
        {
            //Debug.Log("No allies");
            if (anyEnemyNearby)
            {
                //Debug.Log("Enemy spotted - Attack");
                target.Attack(enemyUnits[0]);
            }
            else
            {
                //Debug.Log("No enemy - Move");
                MoveToTarget(target, closestEnemy);
            }
            return;
        }

        if(!anyAllyNearby && !anyEnemyNearby)
        {
            //Debug.Log("No one nearby - Move");
            MoveToTarget(target, (minEnemyDistance < minAllyDistance) ? closestEnemy : closestAlly);
            return;
        }

        if(anyAllyNearby && !anyEnemyNearby)
        {
            //Debug.Log("Allies nearby, enemies not");
            bool rand = generator.GenerateInt(0, 2) > 0;
            if (rand)
            {
                //Debug.Log("Move to enemy");
                MoveToTarget(target, closestEnemy);
            }
            else
            {
                //Debug.Log("Buff ally");
                target.Act(allyUnits);
            }
            
            return;
        }

        if(!anyAllyNearby && anyEnemyNearby)
        {
            Debug.Log("Enemies nearby, allies not");
            for (int i = 0; i < enemyUnits.Count; i++)
            {
                if (enemyUnits[i].GetUnitType() == UnitType.Attack)
                {
                    Debug.Log("Attack unit present - Attack");
                    target.Attack(enemyUnits[i]);
                    return;
                }
            }

            if (enemyUnits.Count > 1)
            {
                Debug.Log("Attack first");
                target.Attack(enemyUnits[0]);
            }
            else
            {
                Debug.Log("Move to ally");
                MoveToTarget(target, closestAlly);
            }

            return;
        }

        if(anyAllyNearby && anyEnemyNearby)
        {
            Debug.Log("Enemies nearby, allies nearby");
            for (int i = 0; i < allyUnits.Count; i++)
            {
                if(allyUnits[i].GetUnitType() == UnitType.Attack)
                {
                    Debug.Log("Attack unit present - Act");
                    target.Act(allyUnits);
                    return;
                }
            }

            if (target.GetUnitType() == UnitType.Buff)
            {
                Debug.Log("Act");
                target.Act(allyUnits);
            }
            else
            {
                Debug.Log("Attack first");
                target.Attack(enemyUnits[0]);
            }
        }
    }

    private void MoveToTarget(Unit activeUnit, Unit moveTarget)
    {
        FieldCell moveCell = null;

        List<FieldCell> neighbours = cellSystem.GetNeighbours(activeUnit.GetUnitCell());
        float minDistance = 100f;
        float currentDistance;

        for(int i = 0; i < neighbours.Count; i++)
        {
            currentDistance = unitSystem.CheckUnitsDistance(moveTarget.GetUnitCell(), neighbours[i]);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                moveCell = neighbours[i];
            }
        }

        cellSystem.ReplaceLinkedCell(activeUnit.GetUnitCell(), moveCell);
        activeUnit.Move(moveCell);
    }

    public void InitBot(UnitSystem units, FieldCellSystem cells)
    {
        unitSystem = units;
        cellSystem = cells;
    }

    public void SetBotCallback(System.Action callback)
    {
        BotCallback = callback;
    }

    public void StartBot()
    {
        InitTurn();

        botCoroutine = ActivateUnits();
        StartCoroutine(botCoroutine);
    }

    public void KillBot()
    {
        if(botCoroutine != null)
        {
            StopCoroutine(botCoroutine);
        }
    }
}
