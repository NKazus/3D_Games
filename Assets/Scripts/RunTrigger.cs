using UnityEngine;
using UnityEngine.UI;
using Zenject;
using FitTheSize.GameServices;

public class RunTrigger : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;

    private Text repText;
    private Text incomeText;

    [Inject] private readonly GameEventHandler events;

    private void Awake()
    {
        repText = restartBg.transform.GetChild(1).GetComponent<Text>();
        incomeText = restartBg.transform.GetChild(2).GetComponent<Text>();
    }

    private void OnEnable()
    {
        events.GameStateEvent += ChangeGameState;
        events.ResultEvent += ShowResult;

        Invoke("Restart", 0.5f);
    }

    private void OnDisable()
    {
        if (IsInvoking())
        {
            CancelInvoke("Restart");
        }
        events.SwitchGameState(false);

        events.ResultEvent -= ShowResult;
        events.GameStateEvent -= ChangeGameState;

        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }

    private void Restart()
    {
        events.SwitchGameState(true);
    }

    private void ChangeGameState(bool isActive)
    {
        if (!isActive)
        {
            restartBg.SetActive(true);
            restart.onClick.AddListener(Restart);
        }
        else
        {
            restartBg.SetActive(false);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void ShowResult(int rep, int money)
    {
        repText.text = rep.ToString();
        incomeText.text = money.ToString();
    }
}
