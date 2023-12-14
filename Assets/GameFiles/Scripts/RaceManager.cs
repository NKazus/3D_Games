using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private Button startButton;

    [SerializeField] private Button pick1Button;
    [SerializeField] private Button pick2Button;

    [SerializeField] private RacePlayer player1;
    [SerializeField] private RacePlayer player2;

    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject pickPanel;
    [SerializeField] private string resultWin;
    [SerializeField] private string resultLose;
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite loseSprite;

    private Text resultText;
    private Image resultImage;

    private int winPlayer;
    private int pickedPlayer;

    private bool finished;

    [Inject] private readonly DataHandler dataHandler;
    [Inject] private readonly RandomGenerator random;
    [Inject] private readonly GlobalEventManager events;

    private void Awake()
    {
        resultImage = resultPanel.transform.GetChild(1).GetComponent<Image>();
        resultText = resultPanel.transform.GetChild(2).GetComponent<Text>();
       
        player1.InitPlayer();
        player2.InitPlayer();
    }

    private void OnEnable()
    {
        dataHandler.UpdateGlobalScore(0);
        dataHandler.UpdateHighlights(0);

        resultPanel.SetActive(false);

        startButton.gameObject.SetActive(true);

        startButton.onClick.AddListener(Play);

        pickPanel.SetActive(false);

        player1.ResetPlayer();
        player2.ResetPlayer();

        pick1Button.onClick.AddListener(() => PickPlayer(1));
        pick2Button.onClick.AddListener(() => PickPlayer(2));
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(Play);

        pick1Button.onClick.RemoveAllListeners();
        pick2Button.onClick.RemoveAllListeners();

        DOTween.Kill("player");
    }

    private void Play()
    {
        resultPanel.SetActive(false);
        startButton.gameObject.SetActive(false);

        float timeInterval = random.GenerateFloat(0.5f, 0.8f);
        float delta = 0.07f * (random.GenerateInt(0, 2) * 2 - 1);
        player1.SetTime(timeInterval);
        player2.SetTime(timeInterval + delta);

        winPlayer = delta > 0 ? 1 : 2;

        finished = false;
        player1.MoveDemo(DemoCallback);
    }

    private void DemoCallback()
    {
        if (!finished)
        {
            finished = true;
            player2.MoveDemo(DemoCallback);
        }
        else
        {
            pickPanel.SetActive(true);
        }
    }

    private void PickPlayer(int value)
    {
        pickPanel.SetActive(false);

        pickedPlayer = value;

        finished = false;
        player1.MoveWay(WayCallback);
        player2.MoveWay(WayCallback);
    }

    private void WayCallback()
    {
        if (!finished)
        {
            finished = true;
        }
        else
        {
            bool win = (winPlayer == pickedPlayer);
            if (win)
            {
                events.PlayBonus();
            }
            else
            {
                events.PlayVibro();
            }
            resultText.text = win ? resultWin : resultLose;
            resultImage.sprite = win ? winSprite : loseSprite;
            resultPanel.SetActive(true);

            dataHandler.UpdateGlobalScore(-5);
            dataHandler.UpdateHighlights(win ? 1 : 0);

            player1.ResetPlayer();
            player2.ResetPlayer();
            startButton.gameObject.SetActive(true);
        }
    }
}
