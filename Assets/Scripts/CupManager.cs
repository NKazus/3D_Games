using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CupManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private DataHandler dataHandler;
    [SerializeField] private Transform dice;
    [SerializeField] private Transform[] cupPositions;
    [SerializeField] private Vector3 diceLandingPosition;
    [SerializeField] private float diceWinPositionY;
    [SerializeField] private CupHandler[] cups;
    [SerializeField] private float[] cupJumpForces;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject pickText;

    [SerializeField] private int maxShuffles = 10;

    private Vector3 initialDicePosition;

    private List<int> cupPositionIndices = new List<int>();
    private List<int> currentCupIndices = new List<int>();

    private int shiftNumber;
    private int shuffles;

    private Action currentShuffleCallback;

    private void Awake()
    {
        initialDicePosition = dice.position;

        for(int i = 0; i < cupPositions.Length; i++)
        {
            cupPositionIndices.Add(i);
        }
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(Activate);
        startButton.gameObject.SetActive(true);
    }


    private void OnDisable()
    {
        DOTween.KillAll();
        ResetCups();
        startButton.onClick.RemoveListener(Activate);
        pickText.SetActive(false);
    }

    private void Activate()
    {
        startButton.gameObject.SetActive(false);
        
        ResetCups();
        RandomGenerator.RandomizeArray(cupJumpForces);
        dice.DOMove(diceLandingPosition, 1.5f).OnComplete(() => {
            cups[0].AttachDice(dice);

            for(int i = 0; i < cups.Length; i++)
            {
                cups[i].SetJumpForce(cupJumpForces[i]);
            }
            CalculatePositions();
            shiftNumber = 0;
            shuffles = 0;
            currentShuffleCallback = CompleteShift;
            Shuffle();
        });
    }

    private void Shuffle()
    {
        shuffles++;
        if (shuffles >= maxShuffles)
        {
            currentShuffleCallback = FinishShifts;
        }

        for(int i = 0; i < cups.Length; i++)
        {
            cups[i].MoveCup(cupPositions[currentCupIndices[i]].position, currentShuffleCallback);
        }
    }

    private void CalculatePositions()
    {
        bool removed;
        int tempIndex;
        for (int i = 0; i < cups.Length; i++)
        {
            tempIndex = currentCupIndices[i];
            if(i > 0)
            {
                cupPositionIndices.Remove(currentCupIndices[i-1]);
            }
            removed = cupPositionIndices.Contains(tempIndex);
            cupPositionIndices.Remove(tempIndex);
            currentCupIndices[i] = cupPositionIndices[RandomGenerator.GenerateInt(0, cupPositionIndices.Count)];
            if (removed)
            {
                cupPositionIndices.Add(tempIndex);
            }
        }
        cupPositionIndices.Clear();
        for (int i = 0; i < cupPositions.Length; i++)
        {
            cupPositionIndices.Add(i);
        }
    }

    private void CompleteShift()
    {
        shiftNumber++;
        if(shiftNumber >= cups.Length)
        {
            shiftNumber = 0;
            CalculatePositions();
            Shuffle();
        }
    }

    private void FinishShifts()
    {
        pickText.SetActive(true);
        shiftNumber++;
        if (shiftNumber >= cups.Length)
        {
            for (int i = 0; i < cups.Length; i++)
            {
                cups[i].ActivateCup(Result);
            }
        }
    }

    private void Result(bool isTreasure)
    {
        pickText.SetActive(false);
        for (int i = 0; i < cups.Length; i++)
        {
            cups[i].DeactivateCup();
        }
        dice.DOMove(new Vector3(dice.position.x, diceWinPositionY, dice.position.z), 1.5f);
        int reward = isTreasure ? 2 : -5;
        if (isTreasure)
        {
            
            GlobalEventManager.PlayBonus();
        }
        dataHandler.UpdateGlobalScore(reward);
        startButton.gameObject.SetActive(true);
    }

    private void ResetCups()
    {
        currentCupIndices.Clear();
        for (int i = 0; i < cups.Length; i++)
        {
            currentCupIndices.Add(i);
        }

        dice.SetParent(null);
        dice.position = initialDicePosition;

        for (int i = 0; i < cups.Length; i++)
        {
            cups[i].ResetCup();
        }
    }
}
