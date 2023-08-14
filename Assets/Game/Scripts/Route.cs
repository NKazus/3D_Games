using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [SerializeField] private Cell[] cells;

    private int routeLenght;
    private int currentCell;

    private void Awake()
    {
        routeLenght = cells.Length;
    }

    public void ResetRoute()
    {
        currentCell = 0;
    }

    public Vector3 GetNext()
    {
        if(++currentCell >= routeLenght)
        {
            currentCell = 0;
        }
        return GetCurrent();
    }

    public Vector3 GetCurrent()
    {
        return cells[currentCell].GetCellPosition();
    }

    public bool IsFinishing()
    {
        return currentCell == 0;
    }

    public void GenerateBuffs()
    {

    }
}
