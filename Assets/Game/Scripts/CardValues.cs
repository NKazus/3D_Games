using UnityEngine;

public class CardValues : MonoBehaviour
{
    [SerializeField] private Card[] cards;

    public int GetCardsQuantity()
    {
        return cards.Length;
    }

    public Card GetCardByNumber(int number)
    {
        return cards[number];
    }
}
