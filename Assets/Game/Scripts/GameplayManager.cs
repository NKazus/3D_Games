using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private Text turnsUI; // if turns <= 0 lose
    [SerializeField] private CardHandler card;


    private const int CARDS_NUMBER = 15; 
    private int[] cardValues;
    private int currentCard;

    [Inject] private readonly EventManager events;
    [Inject] private readonly Resources resources;
    [Inject] private readonly Randomizer randomizer;

    private void Awake()
    {
        cardValues = new int[CARDS_NUMBER];
    }

    private void OnEnable()
    {
        resources.UpdatePlayerScore(0);
        

        events.GameStateEvent += Activate;
    }

    private void OnDisable()
    {
        events.GameStateEvent -= Activate;
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            currentCard = 0;
            turnsUI.text = (CARDS_NUMBER - currentCard).ToString();
            card.Activate(cardValues[currentCard], CardCallback);
        }
        else
        {

        }
    }

    private void GenerateCard()
    {

    }

    private void CardCallback()
    {
        currentCard++;
        turnsUI.text = (CARDS_NUMBER - currentCard).ToString();

    }

    private void ChooseOption(bool add)
    {
        card.Activate(cardValues[currentCard], CardCallback);
    }

}
