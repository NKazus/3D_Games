using System;
using System.Collections.Generic;
using UnityEngine;

public struct TurnScore
{
    public int value;
    public int sum;

    public TurnScore(int value, int sum)
    {
        this.value = value;
        this.sum = sum;
    }

    public override bool Equals(object obj)
    {
        return value == ((TurnScore)obj).value;
    }
}
public class Bot : MonoBehaviour
{
    [SerializeField] private DiceHandler[] botDices;

    private Action<TurnScore> roundCallback;
    private List<int> botCombos;

    private int diceCount;
    private int throwNumber;
    private int maxThrows;

    private List<TurnScore> turnValues;
    private TurnScore[] values;

    private void Awake()
    {
        turnValues = new List<TurnScore>();
        botCombos = new List<int>();
        values = new TurnScore[botDices.Length];
    }

    public void ActivateBot(Action<TurnScore> botCallback, int throws)
    {
        roundCallback = botCallback;
        maxThrows = throws;
        throwNumber = 0;
        ThrowDices();
    }

    public void ResetBot()
    {
        for (int i = 0; i < botDices.Length; i++)
        {
            botDices[i].ResetDice();
        }
        botCombos.Clear();
        throwNumber = 0;
    }

    private void ThrowDices()
    {
        diceCount = 0;
        for (int i = 0; i < botDices.Length; i++)
        {
            botDices[i].Throw(DiceCallback);
        }
        throwNumber++;
    }

    private void DiceCallback()
    {
        diceCount++;
        if (diceCount >= botDices.Length)
        {
            TurnScore result = CalculateThrow();
            if (throwNumber < maxThrows)
            {
                CalculateLocks(result);
                Invoke("ThrowDices", 1f);
            }
            else
            {
                CalculateRound(result);
            }
        }
    }

    private TurnScore CalculateThrow()
    {
        turnValues.Clear();

        for(int i = 0; i < botDices.Length; i++)
        {
            values[i].value = botDices[i].GetDiceValue();

            if (!turnValues.Contains(values[i]) && !botCombos.Contains(values[i].value))
            {
                turnValues.Add(values[i]);
            }
        }

        if(turnValues.Count < 1)
        {
            return new TurnScore(-1, 0);
        }

        int counter;
        for(int i = 0; i < turnValues.Count; i++)
        {
            counter = 0;
            for(int k = 0; k < values.Length; k++)
            {
                if (turnValues[i].Equals(values[k]))
                {
                    counter++;
                }
            }
            turnValues[i] = new TurnScore(turnValues[i].value, turnValues[i].value * counter);
        }

        TurnScore maxSum = turnValues[0];
        for(int i = 0; i < turnValues.Count; i++)
        {
            if(turnValues[i].sum > maxSum.sum)
            {
                maxSum = turnValues[i];
            }
        }

        return maxSum;        
    }

    private void CalculateLocks(TurnScore target)
    {
        for(int i = 0; i < values.Length; i++)
        {
            if(values[i].value == target.value)
            {
                botDices[i].SetLock(true);
            }
            else
            {
                botDices[i].SetLock(false);
            }
        }
    }

    private void CalculateRound(TurnScore result)
    {
        for (int i = 0; i < values.Length; i++)
        {            
            botDices[i].SetLock(false);
        }
        if(result.value > 0)
        {
            botCombos.Add(result.value);
        }
        roundCallback(result);
    }
}
