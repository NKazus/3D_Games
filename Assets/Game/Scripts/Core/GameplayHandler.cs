using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayHandler : MonoBehaviour
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

    [Inject] private readonly GlobalEventManager events;

    #region MONO
    private void Awake()
    {
        restartIcon = restartBg.transform.GetChild(1).GetComponent<Image>();
        restartText = restartBg.transform.GetChild(2).GetComponent<Text>();
        restartExtra = restartBg.transform.GetChild(3).GetComponent<Text>();
    }

    private void OnEnable()
    {
        events.GameStateEvent += ChangeGameState;
        events.WinEvent += ChangeTextToWin;

        Invoke("Restart", 1f);
    }

    private void OnDisable()
    {
        if (IsInvoking())
        {
            CancelInvoke("Restart");
        }
        events.SwitchGameState(false);

        events.WinEvent -= ChangeTextToWin;
        events.GameStateEvent -= ChangeGameState;

        restart.gameObject.SetActive(false);
        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }
    #endregion

    private void Restart()
    {
        events.SwitchGameState(true);
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
        restartExtra.text = "Funding received: "+ points;
        restartExtra.enabled = true;
    }
}
