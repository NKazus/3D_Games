using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameTrigger : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text recordText;


    [Inject] private readonly GameEvents events;

    private void OnEnable()
    {
        events.GameEvent += ChangeGameState;
        events.FinishGameEvent += HandleEndgame;
        restart.onClick.AddListener(Restart);

        Restart();
    }

    private void OnDisable()
    {
        events.TriggerGame(false);
        
        events.FinishGameEvent -= HandleEndgame;
        events.GameEvent -= ChangeGameState;

        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }

    private void Restart()
    {
        events.TriggerGame(true);
    }

    private void ChangeGameState(bool isActive)
    {
        if (!isActive)
        {
            restartBg.SetActive(true);
        }
        else
        {
            restartBg.SetActive(false);
        }
    }

    private void HandleEndgame(int score, bool newRecord)
    {
        scoreText.text = score.ToString();
        recordText.enabled = newRecord;
    }
}
