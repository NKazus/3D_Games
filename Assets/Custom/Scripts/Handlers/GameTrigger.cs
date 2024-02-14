using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameTrigger : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private Sprite win;
    [SerializeField] private string winText;
    [SerializeField] private Sprite lose;
    [SerializeField] private string loseText;

    private Image restartIcon;
    private Text restartText;
    private Text restartExtra;

    [Inject] private readonly GameEvents events;

    #region MONO
    private void Awake()
    {
        restartIcon = restartBg.transform.GetChild(1).GetComponent<Image>();
        restartText = restartBg.transform.GetChild(2).GetComponent<Text>();
        restartExtra = restartBg.transform.GetChild(3).GetComponent<Text>();
    }

    private void OnEnable()
    {
        events.GameEvent += ChangeGameState;
        events.WinEvent += ChangeTextToWin;
        restart.onClick.AddListener(Restart);

        Restart();
    }

    private void OnDisable()
    {
        events.TriggerGame(false);
        
        events.WinEvent -= ChangeTextToWin;
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
            restartExtra.enabled = false;
            restartBg.SetActive(false);
        }
    }

    private void ChangeTextToWin(int points)
    {
        restartIcon.sprite = win;
        restartIcon.SetNativeSize();
        restartText.text = winText;
        restartExtra.text = "You've got "+points.ToString()+" coin(s)!";
        restartExtra.enabled = true;
    }
}
