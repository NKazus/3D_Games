using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Route : MonoBehaviour
{
    [SerializeField] private int routeId;
    [SerializeField] private Cell[] cells;

    private const int WAY_LENGTH = 4;

    private int routeLenght;
    private bool cellPicked;
    private int pickedCell1;
    private int pickedCell2;

    private List<int> gemCellIndices;

    [Inject] private readonly RandomGenerator random;
    [Inject] private readonly GlobalEventManager events;

    private void Awake()
    {
        routeLenght = cells.Length;
        GenerateId();
        gemCellIndices = new List<int>();
    }

    private void ProcessPick(int cell, int route)
    {
        if(route != routeId)
        {
            if (cellPicked)
            {
                cells[pickedCell1].DoHighlight(false, false);
                cellPicked = false;
            }
            return;
        }

        if (!cellPicked)
        {
            cellPicked = true;
            pickedCell1 = cell;
            cells[pickedCell1].DoHighlight(true, false);
        }
        else
        {
            pickedCell2 = cell;
            if(Mathf.Abs(pickedCell1 - pickedCell2) < WAY_LENGTH)
            {
                cellPicked = false;
                ShowWay();
            }
        }
    }

    private void ShowWay()
    {
        int counter = Mathf.Min(pickedCell1, pickedCell2);
        int counterFinish = Mathf.Max(pickedCell1, pickedCell2);
        do
        {
            cells[counter].DoHighlight(true, true);
            cells[counter].ActivateCell(false);
            counter++;
        }
        while (counter <= counterFinish);

        events.CheckHighlight();
    }

    public void ResetRoute()
    {
        for (int i = 0; i < routeLenght; i++)
        {
            cells[i].ResetCell();
        }
    }

    public void ActivateRoute(bool active)
    {
        if (active)
        {
            events.CellEvent += ProcessPick;
        }
        else
        {
            events.CellEvent -= ProcessPick;
        }

        for (int i = 0; i < routeLenght; i++)
        {
            cells[i].ActivateCell(active);
        }
    }

    public void SetGems(List<Gem> gems)
    {
        gemCellIndices.Clear();
        int value;
        int number = gems.Count;

        for(int i = 0; i < number; i++)
        {
            do
            {
                value = random.GenerateInt(0, routeLenght);
            }
            while (gemCellIndices.Contains(value));
            gemCellIndices.Add(value);
            cells[value].SetGem(gems[i]);
        }
    }

    private void GenerateId()
    {
        for(int i = 0; i < routeLenght; i++)
        {
            cells[i].SetId(i, routeId);
        }
    }
}
