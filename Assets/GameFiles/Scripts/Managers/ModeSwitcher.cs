using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ModeSwitcher : MonoBehaviour//reward from magnet evac diff types
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private Sprite win;
    [SerializeField] private string winText;
    [SerializeField] private Sprite lose;
    [SerializeField] private string loseText;

    private Image restartIcon;
    private Text restartText;

    [Inject] private readonly EventHandler events;

    private void Awake()
    {
        restartIcon = restartBg.transform.GetChild(1).GetComponent<Image>();
        restartText = restartBg.transform.GetChild(2).GetComponent<Text>();
    }

    private void OnEnable()
    {
        events.GameModeEvent += ChangeGameState;
        events.WinEvent += ChangeTextToWin;

        Invoke("Restart", 1f);
        restart.onClick.AddListener(Restart);
    }

    private void OnDisable()
    {
        events.SwitchGameMode(false);

        events.WinEvent -= ChangeTextToWin;
        events.GameModeEvent -= ChangeGameState;

        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }

    private void Restart()
    {
        events.SwitchGameMode(true);
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
            restartBg.SetActive(false);
        }
    }

    private void ChangeTextToWin()
    {
        restartIcon.sprite = win;
        restartIcon.SetNativeSize();
        restartText.text = winText;
    }
}
