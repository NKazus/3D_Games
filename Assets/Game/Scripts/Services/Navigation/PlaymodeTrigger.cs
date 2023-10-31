using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlaymodeTrigger : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private Sprite win;
    [SerializeField] private Sprite lose;
    [SerializeField] private string winText;
    [SerializeField] private string loseText;

    private Image restartIcon;
    private Text restartText;

    [Inject] private readonly GameGlobalEvents globalEvents;

    #region MONO
    private void Awake()
    {
        restartIcon = restartBg.transform.GetChild(1).GetComponent<Image>();
        restartText = restartBg.transform.GetChild(2).GetComponent<Text>();
    }

    private void OnEnable()
    {
        globalEvents.GameStateEvent += ChangeGameState;
        globalEvents.WinEvent += ChangeTextToWin;

        Invoke("Restart", 0.5f);
    }

    private void OnDisable()
    {
        globalEvents.SwitchGame(false);
    
        globalEvents.WinEvent -= ChangeTextToWin;
        globalEvents.GameStateEvent -= ChangeGameState;

        restart.gameObject.SetActive(false);
        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }
    #endregion

    private void Restart()
    {
        globalEvents.SwitchGame(true);
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
            restartText.text = loseText;
            restartIcon.SetNativeSize();
            restartBg.SetActive(false);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void ChangeTextToWin()
    {
        restartIcon.sprite = win;
        restartIcon.SetNativeSize();
        restartText.text = winText;
    }
}
