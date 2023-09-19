using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public struct ColorOption
{
    public Color optionColor;
    public bool complementary;
}

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private Text turnsUI; 
    [SerializeField] private CardHandler card;
    [SerializeField] private CardHouse cardHouse;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button[] options;
    [SerializeField] private Button inspiration;
    [SerializeField] private Sprite inspirationOn;
    [SerializeField] private Sprite inspirationOff;

    private const int CARDS_NUMBER = 15; 
    private int currentCard;
    private float currentHSVHue;
    private ColorOption[] colorOptions;

    private bool activeInspiration;

    [Inject] private readonly EventManager events;
    [Inject] private readonly Resources resources;
    [Inject] private readonly Randomizer randomizer;

    private void Awake()
    {
        colorOptions = new ColorOption[options.Length];
    }

    private void OnEnable()
    {
        resources.UpdatePlayerScore(0);
        resources.UpdateInspiration(0);
        optionsPanel.SetActive(false);
        turnsUI.enabled = false;
        inspiration.image.enabled = false;

        events.GameStateEvent += Activate;
        inspiration.onClick.AddListener(SwitchInspiration);
    }

    private void OnDisable()
    {
        events.GameStateEvent -= Activate;

        for (int i = 0; i < options.Length; i++)
        {
            options[i].onClick.RemoveAllListeners();
        }
        inspiration.onClick.RemoveListener(SwitchInspiration);
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            currentCard = 0;
            turnsUI.text = (CARDS_NUMBER - currentCard).ToString()+"/15";
            turnsUI.enabled = true;

            activeInspiration = true;
            SwitchInspiration();
            inspiration.image.enabled = true;
            CheckInspiration();
            

            cardHouse.ResetHouse(true);

            for (int i = 0; i < options.Length; i++)
            {
                options[i].image.color = Color.gray;
                options[i].interactable = false;
            }
            optionsPanel.SetActive(true);
            GenerateCard();
        }
        else
        {
            optionsPanel.SetActive(false);
            turnsUI.enabled = false;
            inspiration.image.enabled = false;
        }
    }

    private void GenerateCard()
    {
        currentHSVHue = randomizer.GenerateFloat(0f, 1f);
        card.Activate(Color.HSVToRGB(currentHSVHue, 1f, 1f), CardCallback);
    }

    private void GenerateOptions()
    {
        colorOptions[0].complementary = true;
        float optionHue = (currentHSVHue + 0.5f) % 1f;
        colorOptions[0].optionColor = Color.HSVToRGB(optionHue, 1f, 1f);

        float axisDir = 1f;
        for (int i = 1; i < colorOptions.Length; i++)
        {
            axisDir = -axisDir;
            colorOptions[i].optionColor =
                Color.HSVToRGB((optionHue + axisDir * randomizer.GenerateFloat(0.1f, 0.2f)) % 1f, 1f, 1f);
            colorOptions[i].complementary = false;
        }
        
        randomizer.RandomizeArray(colorOptions);

        for (int i = 0; i < options.Length; i++)
        {
            options[i].image.color = colorOptions[i].optionColor;
            bool rightOption = colorOptions[i].complementary;
            Debug.Log(rightOption);
            options[i].onClick.AddListener(() => ChooseOption(rightOption));
        }

        SwitchOptions(true);
    }

    private void CardCallback()
    {
        currentCard++;
        turnsUI.text = (CARDS_NUMBER - currentCard).ToString()+"/15";

        GenerateOptions();
    }

    private void SwitchOptions(bool activate)
    {
        for(int i = 0; i < options.Length; i++)
        {
            options[i].interactable = activate;
        }
    }

    private void ChooseOption(bool correct)
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].onClick.RemoveAllListeners();
        }
        SwitchOptions(false);

        if (correct)
        {
            card.SetColor();
            if (cardHouse.AddCard())
            {
                Invoke("CompleteHouse", 1.5f);
                return;
            }
        }
        else
        {
            if (activeInspiration)
            {
                resources.UpdateInspiration(-1);
                SwitchInspiration();
                CheckInspiration();
            }
            else
            {
                cardHouse.ResetHouse(false);
            }
        }

        if (currentCard >= CARDS_NUMBER)
        {
            events.SwitchGameState(false);
            return;
        }
        GenerateCard();
    }

    private void SwitchInspiration()
    {
        activeInspiration = !activeInspiration;
        inspiration.image.sprite = activeInspiration ? inspirationOn : inspirationOff;
    }

    private void CheckInspiration()
    {
        inspiration.interactable = resources.Inspiration > 0;
    }

    private void CompleteHouse()
    {
        resources.UpdatePlayerScore(10);
        events.DoWin();
        events.SwitchGameState(false);
    }
}
