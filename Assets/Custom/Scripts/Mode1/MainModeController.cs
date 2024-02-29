using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainModeController : MonoBehaviour
{
    [SerializeField] private Wall[] arenaWalls;
    [SerializeField] private Ball ball;
    [SerializeField] private WallSpawner spawner;
    [SerializeField] private InputHandler inputHandler;

    [SerializeField] private Button startButton;
    [SerializeField] private Text scoreUI;

    private int currentScore;

    [Inject] private readonly GameEvents events;
    [Inject] private readonly GameDataManager dataManager;

    private void Awake()
    {
        ball.SetCollisionCallback(HandleCollision);
        inputHandler.SetInputCallback(HandleInput);
    }

    private void OnEnable()
    {
        dataManager.RefreshData(DataType.MainScore);
        ChangeFieldState(true);
        events.GameEvent += ChangeFieldState;

        startButton.onClick.AddListener(StartGame);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(StartGame);

        events.GameEvent -= ChangeFieldState;
    }

    private void ChangeFieldState(bool activate)
    {
        if (activate)
        {
            spawner.ResetWalls();

            for(int i = 0; i < arenaWalls.Length; i++)
            {
                arenaWalls[i].ResetWall();
            }
            startButton.gameObject.SetActive(true);

            currentScore = 0;
            scoreUI.text = currentScore.ToString();
        }
        else
        {
            inputHandler.SwitchInput(false);
        }
    }

    private void StartGame()
    {
        startButton.gameObject.SetActive(false);

        inputHandler.SwitchInput(true);
        ball.StartBall();
    }

    private void HandleInput(Vector3 targetPos)
    {
        spawner.SpawnWall(targetPos);
    }

    private void HandleCollision(bool crash)
    {
        if (crash)
        {
            Crash();
            return;
        }

        currentScore++;
        scoreUI.text = currentScore.ToString();
    }

    private void Crash()
    {
        if(dataManager.GetData(DataType.MainScore) < currentScore)
        {
            dataManager.UpdateData(DataType.MainScore, currentScore);
            events.SetFinish(currentScore, true);
        }
        else
        {
            events.SetFinish(currentScore, false);
        }
        events.TriggerGame(false);
    }
}
