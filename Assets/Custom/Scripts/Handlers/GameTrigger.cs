using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameTrigger : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private Sprite win;
    [SerializeField] private string winText;
    [SerializeField] private Color winColor;
    [SerializeField] private Sprite lose;
    [SerializeField] private string loseText;
    [SerializeField] private Color loseColor;

    private Image restartIcon;
    private Text restartText;

    [Inject] private readonly GameEvents events;

    #region MONO
    private void Awake()
    {
        restartIcon = restartBg.transform.GetChild(1).GetComponent<Image>();
        restartText = restartBg.transform.GetChild(2).GetComponent<Text>();
    }

    private void OnEnable()
    {
        events.GameEvent += ChangeGameState;
        events.WinEvent += HandleWin;
        restart.onClick.AddListener(Restart);

        Restart();
    }

    private void OnDisable()
    {
        events.TriggerGame(false);
        
        events.WinEvent -= HandleWin;
        events.GameEvent -= ChangeGameState;

        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }
    #endregion

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
            restartIcon.sprite = lose;
            restartIcon.SetNativeSize();
            restartText.text = loseText;
            restartText.color = loseColor;
            restartBg.SetActive(false);
        }
    }

    private void HandleWin()
    {
        restartIcon.sprite = win;
        restartIcon.SetNativeSize();
        restartText.text = winText;
        restartText.color = winColor;
    }
}
