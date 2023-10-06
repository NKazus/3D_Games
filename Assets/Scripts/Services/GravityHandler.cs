using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GravityHandler : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private string winText;
    [SerializeField] private string loseText;

    private Text restartText;
    private Text restartExtra;

    [Inject] private readonly GlobalEvents events;

    #region MONO
    private void Awake()
    {
        restartText = restartBg.transform.GetChild(2).GetComponent<Text>();
        restartExtra = restartBg.transform.GetChild(3).GetComponent<Text>();
    }

    private void OnEnable()
    {
        events.GameEvent += ChangeGameState;
        events.WinEvent += ChangeTextToWin;

        Invoke("Restart", 1f);
    }

    private void OnDisable()
    {
        if (IsInvoking())
        {
            CancelInvoke("Restart");
        }
        events.SwitchGravityMode(false);

        events.WinEvent -= ChangeTextToWin;
        events.GameEvent -= ChangeGameState;

        restart.gameObject.SetActive(false);
        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }
    #endregion

    private void Restart()
    {
        events.SwitchGravityMode(true);
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
        restartExtra.text = "Points gained: "+ points;
        restartExtra.enabled = true;
    }
}
