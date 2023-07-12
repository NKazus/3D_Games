using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSet : MonoBehaviour
{
    [SerializeField] private DataHandler dataHandler;
    [SerializeField] private CardHandler[] cardObjects;
    [SerializeField] private CardValues cards;
    [SerializeField] private Button[] pickButtons;
    [SerializeField] private Button tryButton;
    [SerializeField] private Button checkButton;
    [SerializeField] private GameObject pickPanel;

    private int cardsQuantity;
    private List<int> targetCards = new List<int>();
    private List<int> pickValues = new List<int>();
    private List<int> pickedCards = new List<int>();
    private List<int> rightCards = new List<int>();

    private void Awake()
    {
        cardsQuantity = cards.GetCardsQuantity();
    }

    private void OnEnable()
    {
        dataHandler.UpdateScore(0);
        for(int i = 0; i < cardObjects.Length; i++)
        {
            cardObjects[i].SwitchCard(cards.GetCardByNumber(cardsQuantity - 1));
        }

        GlobalEventManager.GameStateEvent += Activate;
        tryButton.onClick.AddListener(TryCards);
        checkButton.onClick.AddListener(CheckCards);
        tryButton.gameObject.SetActive(false);
        checkButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (IsInvoking("ShowPickPanel"))
        {
            CancelInvoke("ShowPickPanel");
        }
        GlobalEventManager.GameStateEvent -= Activate;

        tryButton.onClick.RemoveListener(TryCards);
        checkButton.onClick.RemoveListener(CheckCards);
        
        for(int i = 0; i < pickButtons.Length; i++)
        {
            pickButtons[i].onClick.RemoveAllListeners();
            pickButtons[i].image.color = Color.white;
        }
        pickPanel.SetActive(false);
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            for (int i = 0; i < pickButtons.Length; i++)
            {
                pickButtons[i].onClick.RemoveAllListeners();
                pickButtons[i].image.color = Color.white;
            }
            pickPanel.SetActive(false);
            tryButton.gameObject.SetActive(true);
            targetCards.Clear();
            pickedCards.Clear();
            pickValues.Clear();
            rightCards.Clear();
            for (int i = 0; i < cardObjects.Length; i++)
            {
                cardObjects[i].SwitchCard(cards.GetCardByNumber(cardsQuantity - 1));
            }
        }
        else
        {
            checkButton.gameObject.SetActive(false);
        }
    }

    private void TryCards()
    {
        tryButton.gameObject.SetActive(false);

        int cardValue;
        for(int i = 0; i < cardObjects.Length; i++)
        {
            do
            {
                cardValue = RandomGenerator.GenerateInt(0, cardsQuantity - 1);
            }
            while (targetCards.Contains(cardValue));
            targetCards.Add(cardValue);
            cardObjects[i].SwitchCard(cards.GetCardByNumber(cardValue), true);
        }
        Invoke("ShowPickPanel", 10f);
    }

    private void ShowPickPanel()
    {
        for (int i = 0; i < cardObjects.Length; i++)
        {
            cardObjects[i].SwitchCard(cards.GetCardByNumber(cardsQuantity - 1), true);
        }
        GeneratePicks();
        pickPanel.SetActive(true);
      
        checkButton.gameObject.SetActive(true);
    }

    private void GeneratePicks()
    {
        int repeats = RandomGenerator.GenerateInt(1, 4);

        int currentValue;
        for(int i = 0; i < repeats; i++)
        {
            do
            {
                currentValue = RandomGenerator.GenerateInt(0, targetCards.Count);
            }
            while (pickValues.Contains(targetCards[currentValue]));
            pickValues.Add(targetCards[currentValue]);
        }

        for(int i = repeats; i < pickButtons.Length; i++)
        {
            do
            {
                currentValue = RandomGenerator.GenerateInt(0, cardsQuantity - 1);
            }
            while (pickValues.Contains(currentValue));
            pickValues.Add(currentValue);
            rightCards.Add(currentValue);
        }

        RandomGenerator.ShuffleList(pickValues);
        SetPickListeners();
    }

    private void SetPickListeners()
    {
        for(int i = 0; i < pickButtons.Length; i++)
        {
            pickButtons[i].image.sprite = cards.GetCardByNumber(pickValues[i]).image;
            Button currentButton = pickButtons[i];
            int currentValue = pickValues[i];
            pickButtons[i].onClick.AddListener(() => { PickCard(currentButton, currentValue); });
        }
    }

    private void PickCard(Button source, int cardId)
    {        
        if (pickedCards.Contains(cardId))
        {
            pickedCards.Remove(cardId);
            source.image.color = Color.white;
        }
        else
        {
            pickedCards.Add(cardId);
            source.image.color = Color.gray;
        }
    }

    private void CheckCards()
    {
        checkButton.gameObject.SetActive(false);

        for (int i = 0; i < pickButtons.Length; i++)
        {
            pickButtons[i].onClick.RemoveAllListeners();
            pickButtons[i].image.color = Color.white;
        }

        int rightChoice = 0;
        int miss = 0;
        int wrongChoice = 0;
        for(int i = 0; i < pickValues.Count; i++)
        {
            if (rightCards.Contains(pickValues[i]))
            {
                if (pickedCards.Contains(pickValues[i]))
                {
                    rightChoice++;
                    pickButtons[i].image.color = Color.cyan;
                }
                else
                {
                    miss++;
                    pickButtons[i].image.color = Color.gray;
                }
            }
            else
            {
                if (pickedCards.Contains(pickValues[i]))
                {
                    wrongChoice++;
                    pickButtons[i].image.color = Color.yellow;
                }
            }
        }

        int totalScore = (int) ((1.5f * rightChoice) - (float)wrongChoice - (0.5f * miss));
        GlobalEventManager.DoWin(rightChoice, miss, wrongChoice, totalScore);
        GlobalEventManager.PlayReward();
        GlobalEventManager.PlayVibro();
        dataHandler.UpdateScore(totalScore);
        GlobalEventManager.SwitchGameState(false);
    }
}
