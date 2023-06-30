using System;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour //5 throws with blocks => result
{
    [SerializeField] private DiceHandler[] botDices;

    private Action<int> roundCallback;
    private List<int> botCombos;

    private int diceCount;
    private int throwNumber;
    private int maxThrows;

    private void Awake()
    {
        botCombos = new List<int>();
    }

    public void ActivateBot(Action<int> botCallback, int throws)
    {
        roundCallback = botCallback;
        maxThrows = throws;
        Debug.Log("bot turn");
        //ThrowDices();
    }

    public void ResetBot()
    {
        for (int i = 0; i < botDices.Length; i++)
        {
            botDices[i].ResetDice();
        }
        botCombos.Clear();
        throwNumber = 0;
        //ui
    }

    private void ThrowDices()
    {
        diceCount = 0;
        for (int i = 0; i < botDices.Length; i++)
        {
            botDices[i].Throw(DiceCallback);
        }
        throwNumber++;
        //scoreManager.UpdateValues(2, diceNumber);
    }

    private void DiceCallback()
    {
        diceCount++;
        if (diceCount >= botDices.Length)
        {
            if (throwNumber < maxThrows)
            {
                CalculateThrow();
            }
            else
            {
                CalculateRound();
            }
        }
    }

    private void CalculateThrow()
    {
        //locks

        ThrowDices();
    }

    private void CalculateRound()
    {
        int roundScore = 0;
        //best score
        //set combo + ui
        roundCallback(roundScore);
    }
}
