using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHouseLayer : MonoBehaviour
{
    [SerializeField] private CardHouseElement[] layerCards;
    [SerializeField] private CardHouseElement[] layerCaps;

    private int currentCard;

    public void ResetLayer()
    {

    }

    public void DestroyLayer()
    {
        //delete all cards from 0 to current and clear
    }

    public void CompleteLayer()
    {
        //automatically place horizontal cards
    }

    public bool AddElement()
    {
        //add
        return true;//if layer is finished
    }
}
