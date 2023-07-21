using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private Button startButton;
    [SerializeField] private GameObject timer;
    [SerializeField] private float initialTime = 5f;

    private int targetValue;
    private int playerValue;

    private float timeLeft;

    [Inject] private readonly ResourceHandler resources;
    [Inject] private readonly EventManager eventManager;

    private void OnEnable()
    {
        scoreManager.UpdateScore(resources.GlobalScore);
        timer.SetActive(false);
        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(Initialize);
    }

    private void OnDisable()
    {
        eventManager.GameStateEvent -= Activate;

        startButton.onClick.RemoveAllListeners();
        StopAllCoroutines();
    }

    private void Initialize()
    {
        startButton.onClick.RemoveListener(Initialize);
        startButton.gameObject.SetActive(false);
        Activate(true);
        eventManager.GameStateEvent += Activate;
        
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            timer.SetActive(false);

        }
    }

    private void RollCallback(int id)
    {
        timeLeft = initialTime + resources.BonusTime;
        timer.SetActive(true);

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

            Calculate();
        }
    }

    private void Calculate()
    {
        if(playerValue == targetValue)
        {
            eventManager.DoWin();
            resources.UpdateGlobalScore(10);
            eventManager.PlayReward();
            resources.SetBonusTime(true);            
        }
        else
        {
            resources.UpdateGlobalScore(-10);
            eventManager.PlayVibro();
        }
        scoreManager.UpdateScore(resources.GlobalScore);
        eventManager.SwitchGameState(false);
    }
}
