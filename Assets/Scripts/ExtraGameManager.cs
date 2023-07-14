using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ExtraGameManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private DataHandler dataHandler;
    [SerializeField] private Button startButton;
    [SerializeField] private ExtraDiceHandler player;
    [SerializeField] private ExtraDiceHandler bot;
    [SerializeField] private TextureSlider botBoard;
    [SerializeField] private TextureSlider playerBoard;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject hint;


    [SerializeField] private float time = 10f;

    private float timeLeft;

    private int playerScore;
    private int botScore;
    private Transform winPanelTransform;
    private Transform hintTransform;

    private void Awake()
    {
        winPanelTransform = winPanel.transform;
        hintTransform = hint.transform;
    }

    private void OnEnable()
    {
        scoreManager.UpdateValues(0, dataHandler.GlobalScore);

        playerScore = 0;
        scoreManager.UpdateValues(5, playerScore);
        botScore = 0;
        scoreManager.UpdateValues(6, botScore);
        timeLeft = time;
        scoreManager.UpdateTimer(timeLeft);

        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(StartGame);

        botBoard.SetActive(false);
        playerBoard.SetActive(false);
    }

    private void OnDisable()
    {
        winPanel.SetActive(false);
        hint.SetActive(false);
        startButton.onClick.RemoveAllListeners();
        StopAllCoroutines();
        player.ResetState();
        bot.ResetState();
    }

    private void StartGame()
    {
        botBoard.SetActive(true);
        playerBoard.SetActive(true);

        winPanelTransform.DOScale(Vector3.zero, 0.5f).OnComplete(() => winPanel.SetActive(false));
        hint.SetActive(true);
        hintTransform.DOScale(new Vector3(0.7f, 0.6f, 1), 0.5f);

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
        botBoard.SetActive(false);
        playerBoard.SetActive(false);
        hintTransform.DOScale(Vector3.zero, 0.5f).OnComplete(() => hint.SetActive(false));

        if (playerScore >= botScore)
        {
            winPanel.SetActive(true);
            winPanelTransform.DOScale(new Vector3(1, 1, 1), 0.5f);

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
