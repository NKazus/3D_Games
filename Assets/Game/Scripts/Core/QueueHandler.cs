using UnityEngine;
using UnityEngine.UI;

public class QueueHandler : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;

    private Text resultRight;
    private Text resultTotal;

    #region MONO
    private void Awake()
    {
        resultRight = restartBg.transform.GetChild(2).GetComponent<Text>();
        resultTotal = restartBg.transform.GetChild(3).GetComponent<Text>();
    }

    private void OnEnable()
    {
        GlobalEventManager.GameStateEvent += ChangeGameState;
        GlobalEventManager.QueueWinEvent += ChangeTextToWin;

        Invoke("Restart", 1f);
    }

    private void OnDisable()
    {
        if (IsInvoking("Restart"))
        {
            CancelInvoke("Restart");
        }
        GlobalEventManager.SwitchGameState(false);

        GlobalEventManager.QueueWinEvent -= ChangeTextToWin;
        GlobalEventManager.GameStateEvent -= ChangeGameState;

        restart.gameObject.SetActive(false);
        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }
    #endregion

    private void Restart()
    {
        GlobalEventManager.SwitchGameState(true);
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
            restartBg.SetActive(false);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void ChangeTextToWin(int right, int total)
    {
        resultRight.text = "Correct picks:" + right.ToString();
        resultTotal.text = "Total points:" + total.ToString();
    }
}
