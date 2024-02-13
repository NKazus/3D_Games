using System.Collections.Generic;
using UnityEngine;

public class FieldCellSystem : MonoBehaviour
{
    [SerializeField] private FieldGenerator generator;
    [SerializeField] private int fieldSize;

    private FieldCell[,] field;

    private FieldCell[] placements;
    private List<FieldCell> linkedCells = new List<FieldCell>();

    private FieldCell targetCell;

    private System.Action<FieldCell> PickCallback;

    private void PickCell(FieldCell target)
    {
        SwitchNeighbours(false);

        linkedCells.Remove(targetCell);
        targetCell = null;
        linkedCells.Add(target);        

        if(PickCallback != null)
        {
            PickCallback(target);
        }
    }

    private void SwitchNeighbours(bool active)
    {
        int minX, minZ, maxX, maxZ;
        CellIndices targetIndices = targetCell.GetIndices();

        minX = Mathf.Clamp(targetIndices.cellX - 1, 0, fieldSize - 1);
        maxX = Mathf.Clamp(targetIndices.cellX + 1, 0, fieldSize - 1);
        minZ = Mathf.Clamp(targetIndices.cellZ - 1, 0, fieldSize - 1);
        maxZ = Mathf.Clamp(targetIndices.cellZ + 1, 0, fieldSize - 1);

        for(int i = minX; i <= maxX; i++)
        {
            for (int j = minZ; j <= maxZ; j++)
            {
                if (field[i, j] != targetCell && !linkedCells.Contains(field[i, j]))
                {
                    field[i, j].SwitchCell(active);
                }                
            }
        }
    }

    public void InitCells()
    {
        field = generator.Generate(fieldSize);

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                field[i, j].SetCallback(PickCell);
            }
        }
    }

    public void ActivateCells()
    {
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                field[i, j].Activate();
            }
        }
    }

    public void DeactivateCells()
    {
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                field[i, j].Deactivate();
            }
        }
    }

    public void ResetCells()
    {
        targetCell = null;

        linkedCells.Clear();
        for(int i = 0; i < placements.Length; i++)
        {
            linkedCells.Add(placements[i]);
        }


        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                field[i, j].ResetCell();
            }
        }
    }

    public void SwitchCells(FieldCell target)
    {
        if(targetCell == null)
        {
            targetCell = target;
            SwitchNeighbours(true);
            return;
        }

        if(targetCell == target)
        {
            SwitchNeighbours(false);
            targetCell = null;
            return;
        }

        SwitchNeighbours(false);
        targetCell = target;
        SwitchNeighbours(true);
    }

    public void SetCallback(System.Action<FieldCell> callback)
    {
        PickCallback = callback;
    }

    public FieldCell[] GetInitialPlacements(UnitCategory type)
    {
        placements = new FieldCell[System.Enum.GetNames(typeof(UnitType)).Length];
        int zInd = type switch
        {
            UnitCategory.Player => fieldSize - 1,             
            UnitCategory.Bot => 0,
            _ => throw new System.NotSupportedException()
        };

        int mult = fieldSize / placements.Length;
        for(int i = 0; i < placements.Length; i++)
        {
            placements[i] = field[i * mult + 1, zInd];
        }

        return placements;
    }

}
