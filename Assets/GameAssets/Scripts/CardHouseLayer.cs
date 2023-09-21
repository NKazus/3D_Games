using System;
using UnityEngine;

public class CardHouseLayer : MonoBehaviour
{
    [SerializeField] private CardHouseElement[] layerCards;
    [SerializeField] private CardHouseElement[] layerCaps;

    [SerializeField] private ParticleSystem layerEffect;

    private Transform effectTransform;

    private int cardNumber;
    private int capNumber;

    private int currentCard;

    public void InitLayer()
    {
        capNumber = layerCaps.Length;
        cardNumber = layerCards.Length;

        effectTransform = layerEffect.transform;

        for (int i = 0; i < cardNumber; i++)
        {
            layerCards[i].InitElement();
        }
        for (int i = 0; i < capNumber; i++)
        {
            layerCaps[i].InitElement();
        }
    }

    public void ResetLayer(bool fast)
    {
        layerEffect.Stop();
        for (int i = 0; i < cardNumber; i++)
        {
            layerCards[i].HideElement(fast);
        }
        for (int i = 0; i < capNumber; i++)
        {
            layerCaps[i].HideElement(fast);
        }
        if (!fast && currentCard > 0)
        {
            effectTransform.localPosition = layerCards[currentCard / 2].GetPosition();
            layerEffect.Play();
        }
 
        currentCard = 0; 
    }

    public void CompleteLayer()
    {
        for(int i = 0; i < capNumber; i++)
        {
            layerCaps[i].ShowElement(true);
        }
    }

    public bool AddElement()
    {
        bool cardState = (currentCard % 2 == 0);
        if (!cardState)
        {
            layerCards[currentCard - 1].ShowElement(true);
        }
        layerCards[currentCard].ShowElement(!cardState);
        
        currentCard++;
        return currentCard >= layerCards.Length;
    }

    public bool AddElementCallback(Action callback)
    {
        bool cardState = (currentCard % 2 == 0);
        if (!cardState)
        {
            layerCards[currentCard - 1].ShowElement(true);
        }
        layerCards[currentCard].ShowElementCallback(!cardState, callback);

        currentCard++;
        return currentCard >= layerCards.Length;
    }
}
