using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ExtraModeController : MonoBehaviour
{
    [SerializeField] private FloorSystem floor;
    [SerializeField] private WallSystem walls;
    [SerializeField] private ExtraBall ball;

    [SerializeField] private Button startButton;
    [SerializeField] private Text scoreUI;

    [SerializeField] private GameObject finishPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Text finishScoreUI;
    [SerializeField] private Text recordText;

    private int currentScore;

    [Inject] private readonly GameEvents events;
    [Inject] private readonly GameDataManager dataManager;

    private void Awake()
    {
        ball.Init();
        ball.SetCollisionCallback(HandleCollision);
        ball.SetCrashCallback(HandleCrash);

        walls.InitWalls();

        floor.InitFloor();
    }

    private void OnEnable()
    {
        dataManager.RefreshData(DataType.BonusScore);
        ChangeFieldState(true);
        events.GameEvent += ChangeFieldState;

        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(() => ChangeFieldState(true));

        walls.ActivateWalls();
    }

    private void OnDisable()
    {
        floor.DeactivateFloor();
        walls.DeactivateWalls();

        restartButton.onClick.RemoveAllListeners();
        startButton.onClick.RemoveListener(StartGame);

        events.GameEvent -= ChangeFieldState;
    }

    private void ChangeFieldState(bool activate)
    {
        if (activate)
        {
            finishPanel.SetActive(false);

            floor.ResetFloor();
            ball.ResetBall();

            walls.ResetWalls();
            walls.SwitchWalls(false);

            startButton.gameObject.SetActive(true);

            currentScore = 0;
            scoreUI.text = currentScore.ToString();
        }
        else
        {
            finishPanel.SetActive(true);
            walls.SwitchWalls(false);
            floor.DeactivateFloor();
        }
    }

    private void StartGame()
    {
        startButton.gameObject.SetActive(false);

        walls.SwitchWalls(true);
        ball.StartBall();
        floor.ActivateFloor();
    }

    private void HandleCollision()
    {
        events.PlaySound(GameAudio.Bounce);
        currentScore++;
        scoreUI.text = currentScore.ToString();
    }

    private void HandleCrash()
    {
        if (dataManager.GetData(DataType.BonusScore) < currentScore)
        {
            dataManager.UpdateData(DataType.BonusScore, currentScore);
            recordText.enabled = true;
            events.PlaySound(GameAudio.Victory);
        }
        else
        {
            recordText.enabled = false;
            events.PlaySound(GameAudio.Loss);
        }
        finishScoreUI.text = currentScore.ToString();

        ChangeFieldState(false);
    }
}
