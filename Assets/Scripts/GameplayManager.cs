using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private DataHandler dataHandler;
    [SerializeField] private Crystal target;
    [SerializeField] private Crystal player;

    [SerializeField] private Button startButton;
    [SerializeField] private GameObject timer;
    [SerializeField] private float initialTime = 5f;

    private int targetValue;
    private int playerValue;

    private float timeLeft;

    private void OnEnable()
    {
        scoreManager.UpdateScore(dataHandler.GlobalScore);
        timer.SetActive(false);
        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(Initialize);
    }

    private void OnDisable()
    {
        GlobalEventManager.GameStateEvent -= Activate;

        startButton.onClick.RemoveAllListeners();
        StopAllCoroutines();
    }

    private void Initialize()
    {
        startButton.onClick.RemoveListener(Initialize);
        startButton.gameObject.SetActive(false);
        Activate(true);
        GlobalEventManager.GameStateEvent += Activate;
        
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            timer.SetActive(false);
            player.ResetCrystal();
            target.Roll(RollCallback);
        }
    }

    private void RollCallback(int id)
    {
        timeLeft = initialTime + dataHandler.BonusTime;
        timer.SetActive(true);

        player.Activate(SwitchCallback);
        targetValue = id;
        if (gameObject.activeSelf)
        {
            StartCoroutine(StartTimer());
        }
    }

    private void SwitchCallback(int id)
    {
        playerValue = id;
    }

    private IEnumerator StartTimer()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            scoreManager.UpdateTimer(timeLeft);
            yield return null;
        }
        if(timeLeft <= 0)
        {
            player.Deactivate();
            Calculate();
        }
    }

    private void Calculate()
    {
        if(playerValue == targetValue)
        {
            GlobalEventManager.DoWin();
            dataHandler.UpdateGlobalScore(10);
            GlobalEventManager.PlayReward();
            dataHandler.SetBonusTime(true);            
        }
        else
        {
            dataHandler.UpdateGlobalScore(-10);
            GlobalEventManager.PlayVibro();
        }
        scoreManager.UpdateScore(dataHandler.GlobalScore);
        GlobalEventManager.SwitchGameState(false);
    }
}
