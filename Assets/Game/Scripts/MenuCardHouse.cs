using DG.Tweening;
using UnityEngine;

public class MenuCardHouse : MonoBehaviour
{
    [SerializeField] private CardHouseLayer[] layers;

    private int layerNumber;
    private int currentLayer;

    private bool finished;

    private void Awake()
    {
        layerNumber = layers.Length;
        for (int i = 0; i < layerNumber; i++)
        {
            layers[i].InitLayer();
        }
    }

    private void OnEnable()
    {
        finished = false;
        ResetHouse(true);
        AddCard();
    }

    private void OnDisable()
    {
        DOTween.Kill("menu_card_house");
        if (IsInvoking())
        {
            CancelInvoke("AddCard");
        }
    }

    private void AddCard()
    {
        if (layers[currentLayer].AddElementCallback(CardCallback))
        {
            layers[currentLayer].CompleteLayer();
            currentLayer++;
        }

        finished = currentLayer >= layerNumber;
    }

    private void CardCallback()
    {
        if (finished)
        {
            ResetHouse(false);
            Invoke("AddCard", 1.5f);
        }
        else
        {
            AddCard();
        }
    }

    private void ResetHouse(bool fast)
    {        
        for (int i = 0; i < layerNumber; i++)
        {
            layers[i].ResetLayer(fast);
        }
        currentLayer = 0;
    }
}
