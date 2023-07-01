using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private DataHandler dataHandler;

    [SerializeField] private DiceHandler[] userDices;
    [SerializeField] private Bot bot;

    [SerializeField] private int throws;
    [SerializeField] private int rounds;

    [SerializeField] private Button throwButton;
    [SerializeField] private Button skipButton;

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
        //scoreManager.UpdateValues(3, dataHandler.MagicDices);//bonus

        GlobalEventManager.GameStateEvent += Activate;
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
        throwButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);

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

            skipButton.gameObject.SetActive(false);
            throwButton.gameObject.SetActive(true);
            throwButton.onClick.AddListener(ThrowDices);            
        }
    }

    private void ThrowDices()
    {
        throwButton.onClick.RemoveListener(ThrowDices);
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
        if(diceCount >= userDices.Length)
        {
            if(throwNumber < throws)
            {
                throwButton.onClick.AddListener(ThrowDices);
                for (int i = 0; i < userDices.Length; i++)
                {
                    userDices[i].ActivateDice();
                }
            }
            else
            {
                throwButton.gameObject.SetActive(false);
                skipButton.gameObject.SetActive(true);
                skipButton.onClick.AddListener(Skip);
                for (int i = 0; i < userDices.Length; i++)
                {
                    userDices[i].ActivateDice(PickDiceCallback);
                }
            }
        }
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

        userCombos.Add(choice);
        scoreManager.UpdateComboValues(choice, score, true);

        CalculateRound(score);
    }

    private void Skip()
    {
        for (int i = 0; i < userDices.Length; i++)
        {
            userDices[i].DeactivateDice();
        }

        CalculateRound(0);
    }

    private void CalculateRound(int score)
    {
        skipButton.onClick.RemoveListener(Skip);
        skipButton.gameObject.SetActive(false);

        currentUserScore += score;
        scoreManager.UpdateValues(1, currentUserScore);

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

        throwButton.gameObject.SetActive(true);
        throwButton.onClick.AddListener(ThrowDices);
    }

    private void Finish()
    {
        if(currentUserScore >= currentBotScore)
        {
            GlobalEventManager.DoWin(5);
            dataHandler.UpdateGlobalScore(5);
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
