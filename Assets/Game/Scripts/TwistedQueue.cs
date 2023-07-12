using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwistedQueue : MonoBehaviour
{
    [SerializeField] private DataHandler dataHandler;
    [SerializeField] private CardHandler cardObject;
    [SerializeField] private CardValues cards;
    [SerializeField] private Button[] pickButtons;
    [SerializeField] private Button tryButton;
    [SerializeField] private Button checkButton;
    [SerializeField] private GameObject pickPanel;
    [SerializeField] private GameObject spinningCardsPanel;
    [SerializeField] private int queueLength = 5;
    [SerializeField] private Color[] queueColors;


    private int cardsQuantity;
    private int currentQueueElem;
    private List<int> targetCards = new List<int>();
    private List<int> queueValues = new List<int>();
    private List<int> pickedCards = new List<int>();

    private void Awake()
    {
        cardsQuantity = cards.GetCardsQuantity();
    }

    private void OnEnable()
    {
        dataHandler.UpdateScore(0);
        cardObject.ResetCardPosition();

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

        for (int i = 0; i < pickButtons.Length; i++)
        {
            pickButtons[i].onClick.RemoveAllListeners();
            pickButtons[i].image.color = Color.white;
        }

        pickPanel.SetActive(false);
        spinningCardsPanel.SetActive(false);
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            spinningCardsPanel.SetActive(false);
            for (int i = 0; i < pickButtons.Length; i++)
            {
                pickButtons[i].onClick.RemoveAllListeners();
                pickButtons[i].image.color = Color.white;
            }
            pickPanel.SetActive(false);
            tryButton.gameObject.SetActive(true);
            targetCards.Clear();
            pickedCards.Clear();
            queueValues.Clear();
            currentQueueElem = 0;
        }
        else
        {
            checkButton.gameObject.SetActive(false);
        }
    }

    private void TryCards()
    {
        tryButton.gameObject.SetActive(false);

        GeneratePicks();

        int cardValue;
        for (int i = 0; i < queueLength; i++)
        {
            do
            {
                cardValue = RandomGenerator.GenerateInt(0, targetCards.Count);
            }
            while (queueValues.Contains(targetCards[cardValue]));
            queueValues.Add(targetCards[cardValue]);
        }
        ShowCard();
    }

    private void ShowCard()
    {
        cardObject.SwitchCard(cards.GetCardByNumber(queueValues[currentQueueElem]));
        cardObject.ShowCard(ShowCallback);
    }

    private void ShowCallback()
    {
        if(++currentQueueElem >= queueLength)
        {
            ShowPickPanel();
        }
        else
        {
            ShowCard();
        }
    }

    private void ShowPickPanel()
    {
        pickPanel.SetActive(true);
        spinningCardsPanel.SetActive(true);
        checkButton.gameObject.SetActive(true);
    }

    private void GeneratePicks()
    { 
        int currentValue;
        for (int i = 0; i < pickButtons.Length; i++)
        {
            do
            {
                currentValue = RandomGenerator.GenerateInt(0, cardsQuantity - 1);
            }
            while (targetCards.Contains(currentValue));
            targetCards.Add(currentValue);

            pickButtons[i].image.sprite = cards.GetCardByNumber(currentValue).image;
            Button currentButton = pickButtons[i];
            int value = currentValue;
            pickButtons[i].onClick.AddListener(() => { PickCard(currentButton, value); });
        }
    }

    private void PickCard(Button source, int cardId)
    {
        if (pickedCards.Contains(cardId))
        {
            if(pickedCards[0] == cardId)
            {
                pickedCards.Clear();
                for (int i = 0; i < pickButtons.Length; i++)
                {
                    pickButtons[i].image.color = Color.white;
                }
            }
        }
        else
        {
            if(pickedCards.Count < queueLength)
            {
                source.image.color = queueColors[pickedCards.Count];
                pickedCards.Add(cardId);
            }
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
        while (rightChoice < pickedCards.Count && rightChoice < queueValues.Count
            && pickedCards[rightChoice] == queueValues[rightChoice])
        {
            rightChoice++;
        }

        int totalScore = 2 * rightChoice - queueLength;
        GlobalEventManager.DoQueueWin(rightChoice, totalScore);
        GlobalEventManager.PlayReward();
        GlobalEventManager.PlayVibro();
        dataHandler.UpdateScore(totalScore);
        GlobalEventManager.SwitchGameState(false);
    }
}
