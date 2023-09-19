using DG.Tweening;
using UnityEngine;

public class CardHouse : MonoBehaviour
{
    [SerializeField] private CardHouseLayer[] layers;

    private int layerNumber;
    private int currentLayer;

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
        ResetHouse(true);
    }

    public bool AddCard()
    {
        if (layers[currentLayer].AddElement())
        {
            layers[currentLayer].CompleteLayer();
            currentLayer++;
        }

        bool finished = currentLayer >= layerNumber;
        return finished;
    }

    public void ResetHouse(bool full)
    {
        if (full)
        {
            DOTween.Kill("card_house");
            for(int i = 0; i < layerNumber; i++)
            {
                layers[i].ResetLayer(true);
            }
            currentLayer = 0;
        }
        else
        {
            layers[currentLayer].ResetLayer(false);
        }
    }
}
