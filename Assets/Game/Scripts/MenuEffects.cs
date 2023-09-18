using DG.Tweening;
using UnityEngine;
using Zenject;

public class MenuEffects : MonoBehaviour
{
    [SerializeField] private CardHolder[] menuCards;
    [SerializeField] private ParticleSystem[] particles;

    private int currentCard;
    private int cardNumber;

    [Inject] private readonly Randomizer randomizer;

    private void Awake()
    {
        cardNumber = menuCards.Length;
    }

    private void OnEnable()
    {
        currentCard = 0;
        SwitchCard();
    }

    private void OnDisable()
    {
        DOTween.Kill("balance_card");
        for(int i = 0; i < cardNumber; i++)
        {
            particles[i].Stop();
        }
        if (IsInvoking())
        {
            CancelInvoke("SwitchCard");
        }
    }

    private void SwitchCard()
    {
        particles[currentCard].Stop();
        menuCards[currentCard].SetCard(randomizer.GenerateInt(1, 11), CardCallback);
        
    }

    private void CardCallback()
    {
        particles[currentCard].Play();
        currentCard++;
        if(currentCard >= cardNumber)
        {
            currentCard = 0;
        }
        Invoke("SwitchCard", 1f);
    }
}
