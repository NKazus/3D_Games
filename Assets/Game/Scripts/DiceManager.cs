using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private CardHandler cardHandler;
    [SerializeField] private DiceHandler diceHandler;
    [SerializeField] private DataHandler dataHandler;

    [SerializeField] private int defaultDices;
    [SerializeField] private Button diceButton;
    [SerializeField] private Button magicDiceButton;
    [SerializeField] private Button moreButton;
    [SerializeField] private Button lessButton;

    private int currentCardValue;
    private int currentUserValue;
    private int diceNumber;
    private int[] diceValues;

    private bool magicDice = false;

    private void Awake()
    {
        diceValues = new int[defaultDices];
    }

    private void OnEnable()
    {
        scoreManager.UpdateValues(0, dataHandler.GlobalScore);
        scoreManager.UpdateValues(3, dataHandler.MagicDices);

        GlobalEventManager.GameStateEvent += Activate;

        moreButton.gameObject.SetActive(false);
        lessButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        diceHandler.ResetDice();
        GlobalEventManager.GameStateEvent -= Activate;

        diceButton.onClick.RemoveAllListeners();
        magicDiceButton.onClick.RemoveAllListeners();
        moreButton.onClick.RemoveAllListeners();
        lessButton.onClick.RemoveAllListeners();
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            diceHandler.ResetDice();
            diceNumber = defaultDices;
            scoreManager.UpdateValues(2, diceNumber);
            magicDice = false;
            currentUserValue = 0;
            scoreManager.UpdateValues(1, currentUserValue);

            currentCardValue = cardHandler.Activate();

            CalculateDiceValues();

            diceButton.onClick.AddListener(ThrowDice);
            if(dataHandler.MagicDices > 0)
            {
                magicDiceButton.onClick.AddListener(ActivateMagicDice);
            }
        }
    }

    private void CalculateDiceValues()
    {
        int finalValue = currentCardValue - RandomGenerator.GenerateInt(1, defaultDices) - 6;
        diceValues[0] = RandomGenerator.GenerateInt(1,7);
        int halfValue = finalValue / 2;
        diceValues[1] = (halfValue < 6) ? halfValue : 6;
        int remainingValue = finalValue - diceValues[1];
        diceValues[2] = (remainingValue < 6) ? remainingValue : RandomGenerator.GenerateInt(1,7);
        RandomGenerator.RandomizeArray(diceValues);
        /*for(int i = 0; i < defaultDices; i++)
        {
            Debug.Log(diceValues[i]);
        }*/
    }

    private void ThrowDice()
    {
        diceButton.onClick.RemoveListener(ThrowDice);
        diceHandler.Throw(DiceCallback, 0, diceValues[defaultDices - diceNumber]);
        diceNumber--;
        scoreManager.UpdateValues(2, diceNumber);
    }

    private void DiceCallback()
    {
        currentUserValue += diceValues[defaultDices - diceNumber - 1];
        scoreManager.UpdateValues(1, currentUserValue);
        if (diceNumber > 0)
        {
            diceButton.onClick.AddListener(ThrowDice);
        }
        else
        {
            moreButton.gameObject.SetActive(true);
            lessButton.gameObject.SetActive(true);

            moreButton.onClick.AddListener(() => { ThrowMagicDice(1); });
            lessButton.onClick.AddListener(() => { ThrowMagicDice(-1); });
        }
    }

    private void ThrowMagicDice(int choice)
    {
        moreButton.onClick.RemoveAllListeners();
        lessButton.onClick.RemoveAllListeners();
        magicDiceButton.onClick.RemoveAllListeners();

        moreButton.gameObject.SetActive(false);
        lessButton.gameObject.SetActive(false);

        int lastValue = magicDice ? 6 : RandomGenerator.GenerateInt(1,7);
        diceHandler.Throw(Finish, magicDice ? 2 : 1, lastValue);

        if (magicDice)
        {
            dataHandler.ReduceMagicDices();
            scoreManager.UpdateValues(3, dataHandler.MagicDices);
            ActivateMagicDice();
        }

        Calculate(lastValue, choice);
    }

    private void Calculate(int last, int button)
    {
        currentUserValue += last;
        scoreManager.UpdateValues(1, currentUserValue);
        int difference = currentUserValue - currentCardValue;
        int defaultReward = (button > 0) ? 5 : 0;
        if(difference * button >= 0)
        {
            GlobalEventManager.DoWin();
            GlobalEventManager.PlayReward();
            dataHandler.UpdateGlobalScore(defaultReward + difference);
        }
        else
        {
            dataHandler.UpdateGlobalScore(difference);
            GlobalEventManager.PlayVibro();
        }
    }

    private void Finish()
    {
        scoreManager.UpdateValues(0, dataHandler.GlobalScore);
        GlobalEventManager.SwitchGameState(false);
    }

    private void ActivateMagicDice()
    {
        magicDice = !magicDice;
        //active button visual
        if (magicDice)
        {
            magicDiceButton.image.color = Color.red;
        }
        else
        {
            magicDiceButton.image.color = Color.white;
        }
    }
}
