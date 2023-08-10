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
    private Text restartCoins;
    private Text restartSeeds;

    [Inject] private readonly GlobalEventManager eventManager;

    #region MONO
    private void Awake()
    {
        restartText = restartBg.transform.GetChild(1).GetComponent<Text>();
        restartCoins = restartBg.transform.GetChild(2).GetComponent<Text>();
        restartSeeds = restartBg.transform.GetChild(3).GetComponent<Text>();
    }

    private void OnEnable()
    {
        eventManager.GameStateEvent += ChangeGameState;
        eventManager.WinEvent += ChangeTextToWin;

        Invoke("Restart", 1f);
    }

    private void OnDisable()
    {
        if (IsInvoking())
        {
            CancelInvoke("Restart");
        }
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
            restartText.text = loseText;
            restartCoins.text = restartSeeds.text = "+0";
            restartBg.SetActive(false);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void ChangeTextToWin(int coins)
    {
        restartText.text = winText;
        restartCoins.text = "+" + coins.ToString();
        restartSeeds.text = "+1";
    }
}
