using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Route : MonoBehaviour
{
    [SerializeField] private Cell[] cells;

    private int routeLenght;
    private int currentCell;

    private List<int> lockedCells;

    [Inject] private readonly RandomValueProvider random;

    private void Awake()
    {
        routeLenght = cells.Length;
        lockedCells = new List<int>();
    }

    public void ResetRoute()
    {
        currentCell = 0;
        lockedCells.Clear();
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

    public Vector3 GetById(int id)
    {
        return cells[id].GetCellPosition();
    }

    public bool IsFinishing()
    {
        return currentCell == 0;
    }

    public int GenerateId()
    {
        int newId;
        do
        {
            newId = random.GetInt(1, routeLenght);
        }
        while (lockedCells.Contains(newId));
        lockedCells.Add(newId);

        return newId;
    }

    public void RemoveId(int id)
    {
        lockedCells.Remove(id);
    }
}
