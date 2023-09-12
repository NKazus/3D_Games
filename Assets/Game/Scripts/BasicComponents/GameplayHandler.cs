using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayHandler : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private string winText;
    [SerializeField] private string loseText;

    private Text restartText;
    private Text restartExtra;

    [Inject] private readonly GlobalEventManager events;

    #region MONO
    private void Awake()
    {
        restartText = restartBg.transform.GetChild(2).GetComponent<Text>();
        restartExtra = restartBg.transform.GetChild(3).GetComponent<Text>();
    }

    private void OnEnable()
    {
        events.GameStateEvent += ChangeGameState;
        events.WinEvent += ChangeTextToWin;

        Invoke("Restart", 0.5f);
    }

    private void OnDisable()
    {
        if (IsInvoking())
        {
            CancelInvoke("Restart");
        }

        events.WinEvent -= ChangeTextToWin;
        events.GameStateEvent -= ChangeGameState;

        events.SwitchGameState(false);

        ChangeGameState(true);
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
            restartText.text = loseText;
            restartExtra.enabled = false;
            restartBg.SetActive(false);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void ChangeTextToWin(int points)
    {
        restartText.text = winText;
        restartExtra.text = "Funding received: "+ points;
        restartExtra.enabled = true;
    }
}
