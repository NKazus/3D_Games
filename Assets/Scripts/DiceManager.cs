using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private DataHandler dataHandler;

    [SerializeField] private DiceHandler[] userDices;
    [SerializeField] private Bot bot;
    [SerializeField] private TextureSlider playerBoard;
    [SerializeField] private TextureSlider botBoard;

    [SerializeField] private int throws;
    [SerializeField] private int rounds;

    [SerializeField] private Button throwButton;
    [SerializeField] private Button skipButton;

    [SerializeField] private Image multiplyer;
    [SerializeField] private Sprite multActive;
    [SerializeField] private Sprite multInactive;

    private int currentUserScore;
    private int currentBotScore;

    private int throwNumber;
    private int roundNumber;

    private int diceCount = 0;

    private List<int> userCombos;

    private void Awake()
    {
        userCombos = new List<int>();
    }

    private void OnEnable()
    {
        scoreManager.UpdateValues(0, dataHandler.GlobalScore);

        GlobalEventManager.GameStateEvent += Activate;
        botBoard.SetActive(false);
        playerBoard.SetActive(false);
    }

    private void OnDisable()
    {
        for (int i = 0; i < userDices.Length; i++)
        {
            userDices[i].ResetDice();
        }
        bot.ResetBot();
        GlobalEventManager.GameStateEvent -= Activate;

        throwButton.onClick.RemoveAllListeners();
        skipButton.onClick.RemoveAllListeners();

        throwButton.image.color = Color.gray;
        skipButton.image.color = Color.gray;

        throwNumber = 0;
        scoreManager.UpdateValues(3, throwNumber);
        roundNumber = 1;
        scoreManager.UpdateValues(4, roundNumber);
        currentUserScore = 0;
        scoreManager.UpdateValues(1, currentUserScore);
        currentBotScore = 0;
        scoreManager.UpdateValues(2, currentBotScore);
        for (int i = 0; i < 6; i++)
        {
            scoreManager.UpdateComboValues(i + 1, 0);
            scoreManager.UpdateComboValues(i + 1, 0, true);
        }

        multiplyer.sprite = dataHandler.BonusMultiplyer > 1 ? multActive : multInactive;
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            for (int i = 0; i < userDices.Length; i++)
            {
                userDices[i].ResetDice();
            }
            bot.ResetBot();

            throwNumber = 0;
            scoreManager.UpdateValues(3, throwNumber);
            roundNumber = 1;
            scoreManager.UpdateValues(4, roundNumber);
            currentUserScore = 0;
            scoreManager.UpdateValues(1, currentUserScore);
            currentBotScore = 0;
            scoreManager.UpdateValues(2, currentBotScore);
            userCombos.Clear();
            for(int i = 0; i < 6; i++)
            {
                scoreManager.UpdateComboValues(i + 1, 0);
                scoreManager.UpdateComboValues(i + 1, 0, true);
            }

            multiplyer.sprite = dataHandler.BonusMultiplyer > 1 ? multActive : multInactive;

            skipButton.image.DOColor(Color.white, 0.5f);
            skipButton.onClick.AddListener(Skip);

            throwButton.image.DOColor(Color.white, 0.5f);
            throwButton.onClick.AddListener(ThrowDices);

            playerBoard.SetActive(true);
            botBoard.SetActive(false);
        }
    }

    private void ThrowDices()
    {
        throwButton.onClick.RemoveListener(ThrowDices);
        throwButton.image.DOColor(Color.gray, 0.5f);

        skipButton.onClick.RemoveListener(Skip);
        skipButton.image.DOColor(Color.gray, 0.5f);

        diceCount = 0;
        throwNumber++;
        
        for (int i = 0; i < userDices.Length; i++)
        {
            userDices[i].DeactivateDice();
            userDices[i].Throw(DiceCallback);
        }
        scoreManager.UpdateValues(3, throwNumber);
    }

    private void DiceCallback()
    {
        diceCount++;
        if (diceCount >= userDices.Length)
        {
            if (throwNumber < throws)
            {
                throwButton.image.DOColor(Color.white, 0.5f);
                throwButton.onClick.AddListener(ThrowDices);
                for (int i = 0; i < userDices.Length; i++)
                {
                    userDices[i].ActivateDice();
                }
            }
            else
            {
                for (int i = 0; i < userDices.Length; i++)
                {
                    userDices[i].ActivateDice(PickDiceCallback);
                }                
            }

            skipButton.onClick.AddListener(Skip);
            skipButton.image.DOColor(Color.white, 0.5f);
        }
    }

    private bool CheckFullCombo()
    {
        int num = 1;
        int prev = userDices[0].GetDiceValue();
        int current;
        for (int i = 1; i < userDices.Length; i++)
        {
            current = userDices[i].GetDiceValue();
            if (current == prev)
            {
                num++;
            }
            prev = current;
        }
        return num == userDices.Length;
    }

    private void PickDiceCallback(int choice)
    {
        if (userCombos.Contains(choice))
        {
            return;
        }

        int score = 0;

        for (int i = 0; i < userDices.Length; i++)
        {
            userDices[i].DeactivateDice();
            userDices[i].SetLock(false);
            if(userDices[i].GetDiceValue() == choice)
            {
                score += choice;
            }
        }

        score *= dataHandler.BonusMultiplyer;
        userCombos.Add(choice);
        scoreManager.UpdateComboValues(choice, score, true);

        CalculateRound(score);
    }

    private void Skip()
    {
        for (int i = 0; i < userDices.Length; i++)
        {
            userDices[i].DeactivateDice();
            userDices[i].SetLock(false);
        }

        int value = userDices[0].GetDiceValue();
        if (CheckFullCombo() && !userCombos.Contains(value))
        {
            int score = value * userDices.Length * dataHandler.BonusMultiplyer;
            CalculateRound(score);
            userCombos.Add(value);
            scoreManager.UpdateComboValues(value, score, true);
            return;
        }
        CalculateRound(0);
    }

    private void CalculateRound(int score)
    {
        skipButton.onClick.RemoveListener(Skip);
        skipButton.image.DOColor(Color.gray, 0.5f);

        throwButton.onClick.RemoveListener(ThrowDices);
        throwButton.image.DOColor(Color.gray, 0.5f);

        currentUserScore += score;
        scoreManager.UpdateValues(1, currentUserScore);

        botBoard.SetActive(true);
        playerBoard.SetActive(false);

        bot.ActivateBot(BotCallback, throws);
    }

    private void BotCallback(TurnScore botResult)
    {
        if(botResult.value > 0)
        {
            scoreManager.UpdateComboValues(botResult.value, botResult.sum);
        }
        
        currentBotScore += botResult.sum;
        scoreManager.UpdateValues(2, currentBotScore);

        if (roundNumber >= rounds)
        {
            Finish();
            return;
        }

        roundNumber++;
        scoreManager.UpdateValues(4, roundNumber);

        throwNumber = 0;
        scoreManager.UpdateValues(3, throwNumber);

        throwButton.image.DOColor(Color.white, 0.5f);
        throwButton.onClick.AddListener(ThrowDices);

        skipButton.onClick.AddListener(Skip);
        skipButton.image.DOColor(Color.white, 0.5f);

        botBoard.SetActive(false);
        playerBoard.SetActive(true);
    }

    private void Finish()
    {
        if(currentUserScore >= currentBotScore)
        {
            GlobalEventManager.DoWin();
            dataHandler.UpdateGlobalScore(5);
            dataHandler.ActivateBonus(false);
            GlobalEventManager.PlayReward();
        }
        else
        {
            dataHandler.UpdateGlobalScore(-5);
            GlobalEventManager.PlayVibro();
        }
        scoreManager.UpdateValues(0, dataHandler.GlobalScore);
        GlobalEventManager.SwitchGameState(false);
    }
}
