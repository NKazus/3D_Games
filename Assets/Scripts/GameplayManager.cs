using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private RangePanel rangeUI;
    [SerializeField] private Balance balanceComp;
    [SerializeField] private Text turnsUI;
    [SerializeField] private GameObject noSkipText;
    [SerializeField] private Button addButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private CardHandler card;

    [SerializeField] private int[] minPercentValues;
    [SerializeField] private int[] maxPercentValues;
    [SerializeField] private int minAdds = 2;

    private const int CARDS_NUMBER = 6; 
    private int[] cardValues;
    private int cardsSum;
    private int currentCard;
    private int adds;

    private int playerSum;
    private int min;
    private int max;
    private bool skipNext;

    [Inject] private readonly EventManager events;
    [Inject] private readonly Resources resources;
    [Inject] private readonly Randomizer randomizer;

    private void Awake()
    {
        cardValues = new int[CARDS_NUMBER];
        rangeUI.InitRange();
    }

    private void OnEnable()
    {
        resources.UpdateBalanceCharges(true, 0);
        resources.UpdateBalanceCharges(false, 0);

        events.GameStateEvent += Activate;
        addButton.onClick.AddListener(() => ChooseOption(true));
        skipButton.onClick.AddListener(() => ChooseOption(false));
    }

    private void OnDisable()
    {
        events.GameStateEvent -= Activate;

        addButton.onClick.RemoveAllListeners();
        skipButton.onClick.RemoveAllListeners();

        addButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        noSkipText.SetActive(false);
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            currentCard = 0;
            turnsUI.text = (CARDS_NUMBER - currentCard).ToString();
            playerSum = 0;
            adds = 0;
            balanceComp.ResetBalance();
            GenerateCards();
            GenerateRange();
            card.Activate(cardValues[currentCard], CardCallback);
        }
        else
        {

        }
    }

    private void GenerateCards()
    {
        cardsSum = 0;
        for (int i = 0; i < CARDS_NUMBER; i++)
        {
            cardValues[i] = randomizer.GenerateInt(1, 11);
            cardsSum += cardValues[i];
        }
    }

    private void GenerateRange()
    {
        min = minPercentValues[randomizer.GenerateInt(0, minPercentValues.Length)];
        max = maxPercentValues[randomizer.GenerateInt(0, maxPercentValues.Length)];

        rangeUI.ResetRange(min / 100f, max / 100f);
    }

    private void CardCallback()
    {
        currentCard++;
        turnsUI.text = (CARDS_NUMBER - currentCard).ToString();

        if (currentCard % 2 == 1)
        {
            addButton.gameObject.SetActive(true);
            if (adds < minAdds && currentCard == CARDS_NUMBER - 1)
            {
                noSkipText.SetActive(true);
                return;                
            }
            
            skipButton.gameObject.SetActive(true);
        }
        else
        {
            if (!skipNext)
            {
                playerSum += cardValues[currentCard - 1];
                rangeUI.UpdateProgress((float)playerSum / cardsSum);
            }
            if(currentCard >= CARDS_NUMBER)
            {
                balanceComp.LockBalance();
                CalculateResult();
                return;
            }
            card.Activate(cardValues[currentCard], CardCallback);
        }
    }

    private void ChooseOption(bool add)
    {
        addButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        noSkipText.SetActive(false);

        skipNext = !add;
        if (add)
        {
            adds++;
            playerSum += cardValues[currentCard - 1];
            rangeUI.UpdateProgress((float)playerSum / cardsSum);
        }
        card.Activate(cardValues[currentCard], CardCallback);
    }

    private void CalculateResult()
    {
        float minBorder = min / 100f;
        float maxBorder = max / 100f;
        float endValue = balanceComp.DoBalance(playerSum, cardsSum, minBorder, maxBorder);
        float finalPercentage = endValue / cardsSum;

        int sign = (finalPercentage > maxBorder || finalPercentage < minBorder) ? -1 : 1;
        if(sign > 0)
        {
            events.DoWin();
        }

        StartCoroutine(Finish(finalPercentage, (int)endValue * sign));
        
    }

    private IEnumerator Finish(float final, int score)
    {
        yield return new WaitForSeconds(1f);
        balanceComp.ShowProcess(false);
        rangeUI.UpdateProgress(final);
        resources.UpdatePlayerScore(score);
        events.SwitchGameState(false);
    }
}
