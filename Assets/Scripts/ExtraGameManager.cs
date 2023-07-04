using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraGameManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private DataHandler dataHandler;
    [SerializeField] private Button startButton;
    [SerializeField] private ExtraDiceHandler player;
    [SerializeField] private ExtraDiceHandler bot;
    [SerializeField] private GameObject winPanel;

    [SerializeField] private float time = 10f;

    private float timeLeft;

    private int playerScore;
    private int botScore;

    private void OnEnable()
    {
        playerScore = 0;
        scoreManager.UpdateValues(5, playerScore);
        botScore = 0;
        scoreManager.UpdateValues(6, botScore);
        timeLeft = time;
        scoreManager.UpdateTimer(timeLeft);

        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(StartGame);        
    }

    private void OnDisable()
    {
        winPanel.SetActive(false);
        startButton.onClick.RemoveAllListeners();
        StopAllCoroutines();
        player.ResetState();
        bot.ResetState();
    }

    private void StartGame()
    {
        winPanel.SetActive(false);
        startButton.onClick.RemoveListener(StartGame);
        startButton.gameObject.SetActive(false);

        playerScore = 0;
        scoreManager.UpdateValues(5, playerScore);
        botScore = 0;
        scoreManager.UpdateValues(6, botScore);
        timeLeft = time;
        scoreManager.UpdateTimer(timeLeft);

        StartCoroutine(StartTimer());

        player.Activate(PlayerCallback, true);
        bot.Activate(BotCallback, false);
    }

    private IEnumerator StartTimer()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            scoreManager.UpdateTimer(timeLeft);
            yield return null;
        }
        if (timeLeft <= 0)
        {
            player.ResetState();
            bot.ResetState();
            Calculate();
        }
    }

    private void PlayerCallback(int value)
    {
        playerScore += value;
        scoreManager.UpdateValues(5, playerScore);

        player.Activate(PlayerCallback, true);
    }

    private void BotCallback(int value)
    {
        botScore += value;
        scoreManager.UpdateValues(6, botScore);

        bot.Activate(BotCallback, false);
    }

    private void Calculate()
    {
        if(playerScore >= botScore)
        {
            winPanel.SetActive(true);

            dataHandler.ActivateBonus(true);
            GlobalEventManager.PlayMult();
        }
        else
        {
            dataHandler.UpdateGlobalScore(-5);
            scoreManager.UpdateValues(0, dataHandler.GlobalScore);
            GlobalEventManager.PlayVibro();
        }
        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(StartGame);
    }
}
