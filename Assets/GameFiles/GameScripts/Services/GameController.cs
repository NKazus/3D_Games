using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameController : MonoBehaviour
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

    [Inject] private readonly AppEvents eventManager;

    private void Awake()
    {
        restartIcon = restartBg.transform.GetChild(1).GetComponent<Image>();
        restartText = restartBg.transform.GetChild(2).GetComponent<Text>();
        restartExtra = restartBg.transform.GetChild(3).GetComponent<Text>();
    }

    private void OnEnable()
    {
        eventManager.GameEvent += ChangeGameState;
        eventManager.WinEvent += ChangeTextToWin;

        Invoke("Restart", 0.5f);
    }

    private void OnDisable()
    {
        eventManager.DoGame(false);
    
        eventManager.WinEvent -= ChangeTextToWin;
        eventManager.GameEvent -= ChangeGameState;
  
        restart.gameObject.SetActive(false);
        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }

    private void Restart()
    {
        eventManager.DoGame(true);
    }

    private void ChangeGameState(bool isActive)
    {
        if (!isActive)
        {
            restart.gameObject.SetActive(true);
            restartBg.SetActive(true);
            restart.onClick.AddListener(Restart);
        }
        else
        {
            restart.gameObject.SetActive(false);
            restartIcon.sprite = lose;
            restartIcon.SetNativeSize();
            restartText.text = loseText;
            restartExtra.enabled = false;
            restartBg.SetActive(false);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void ChangeTextToWin(int points)
    {
        restartIcon.sprite = win;
        restartIcon.SetNativeSize();
        restartText.text = winText;
        restartExtra.text = "You've mined "+points.ToString()+" ore samples!";
        restartExtra.enabled = true;
    }
}
