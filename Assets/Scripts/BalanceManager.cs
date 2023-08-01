using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BalanceManager : MonoBehaviour
{
    [SerializeField] private Button start;
    [SerializeField] private GameObject[] optionObjects;
    [SerializeField] private CardHolder[] playerCards;
    [SerializeField] private CardHolder[] targetCards;

    [SerializeField] private Text pickText;
    [SerializeField] private GameObject result;
    [SerializeField] private string win10Text;
    [SerializeField] private string win30Text;
    [SerializeField] private string loseText;

    private BalanceButton[] options;
    private List<int> targetValues;
    private List<int> playerValues;
    private Text resultText;

    private int currentChoice;

    [Inject] private readonly Randomizer randomizer;
    [Inject] private readonly Resources resources;

    private void Awake()
    {
        targetValues = new List<int>();
        playerValues = new List<int>();

        options = new BalanceButton[optionObjects.Length];
        for(int i = 0; i < options.Length; i++)
        {
            options[i] = optionObjects[i].GetComponent<BalanceButton>();
        }

        resultText = result.transform.GetChild(1).GetComponent<Text>();
    }

    private void OnEnable()
    {
        start.onClick.AddListener(ActivateBalancingGame);
        pickText.enabled = false;

        resources.UpdatePlayerScore(0);
        resources.UpdateBalanceCharges(true, 0);
        resources.UpdateBalanceCharges(false, 0);
        result.SetActive(false);        

        ResetBalancing();        
    }

    private void OnDisable()
    {
        start.onClick.RemoveListener(ActivateBalancingGame);
    }

    private void ActivateBalancingGame()
    {
        start.gameObject.SetActive(false);
        result.SetActive(false);
        pickText.enabled = true;

        currentChoice = 0;

        ResetCards();
        GenerateValues();

        for (int i = 0; i < playerCards.Length; i++)
        {
            optionObjects[i].SetActive(true);
            options[i].SetButton(targetValues[i], ButtonCallback);
        }
        randomizer.RandomizeList(targetValues);
    }

    private void ResetBalancing()
    {        
        for (int i = 0; i < optionObjects.Length; i++)
        {
            optionObjects[i].SetActive(false);
        }

        start.gameObject.SetActive(true);
    }

    private void ResetCards()
    {
        for (int i = 0; i < playerCards.Length; i++)
        {
            playerCards[i].ResetCard();
            targetCards[i].ResetCard();
        }
    }

    private void GenerateValues()
    {
        targetValues.Clear();
        playerValues.Clear();

        int value;
        for (int i = 0; i < optionObjects.Length; i++)
        {
            do
            {
                value = randomizer.GenerateInt(1, 11);
            }
            while (targetValues.Contains(value));
            targetValues.Add(value);
        }
    }

    private void ButtonCallback(int value)
    {
        for(int i = 0; i < options.Length; i++)
        {
            options[i].ResetButton(true);
        }
        playerValues.Add(value);
        playerCards[currentChoice]
            .SetCard(value, () => { targetCards[currentChoice].SetCard(targetValues[currentChoice], CardCallback); });
    }

    private void CardCallback()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].Activate();
        }

        if (++currentChoice >= optionObjects.Length - 1)
        {
            pickText.enabled = false;
            for (int i = 0; i < optionObjects.Length; i++)
            {
                options[i].ResetButton();
                if (!playerValues.Contains(targetValues[i]))
                {
                    playerValues.Add(targetValues[i]);
                }
            }
            playerCards[currentChoice]
                .SetCard(playerValues[currentChoice], () => { targetCards[currentChoice].SetCard(targetValues[currentChoice], Calculate); });
        }
    }

    private void Calculate()
    {
        int hits = 0;
        for(int i = 0; i < optionObjects.Length; i++)
        {
            if (targetValues[i] == playerValues[i])
            {
                hits++;
            }
        }
        resources.UpdatePlayerScore(-5);

        string resultString = loseText;
        switch (hits)
        {
            case 1: resources.UpdateBalanceCharges(true, 1); resultString = win10Text; break;
            case 3: resources.UpdateBalanceCharges(false, 1); resultString = win30Text; break;
            default: Debug.Log("0"); break;
        }

        resultText.text = resultString;
        result.SetActive(true);
        ResetBalancing();
    }
}
