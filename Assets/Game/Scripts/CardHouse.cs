using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHouse : MonoBehaviour
{
    [SerializeField] private CardHouseLayer[] layers;

    private int currentLayer; //if complete layers.Lenght => win else complete layer

    public void AddCard(Action callback)
    {
        //current layer add card
        //check completion
        //if complete win
        //else callback
    }

    public void ResetHouse()
    {
        for(int i = 0; i < layers.Length; i++)
        {
            layers[i].ResetLayer();
        }
    }
}
