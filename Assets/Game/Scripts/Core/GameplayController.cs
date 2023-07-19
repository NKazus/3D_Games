using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayController : MonoBehaviour
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

    [Inject] private readonly GlobalEventManager eventManager;

    #region MONO
    private void Awake()
    {
        restartIcon = restartBg.transform.GetChild(1).GetComponent<Image>();
        restartText = restartBg.transform.GetChild(2).GetComponent<Text>();
        restartExtra = restartBg.transform.GetChild(3).GetComponent<Text>();
    }

    private void OnEnable()
    {
        eventManager.GameStateEvent += ChangeGameState;
        eventManager.WinEvent += ChangeTextToWin;

        Restart();
    }

    private void OnDisable()
    {
        eventManager.SwitchGameState(false);
    
        eventManager.WinEvent -= ChangeTextToWin;
        eventManager.GameStateEvent -= ChangeGameState;
  
        restart.gameObject.SetActive(false);
        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }
    #endregion

    private void Restart()
    {
        eventManager.SwitchGameState(true);
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
