using UnityEngine;
using UnityEngine.UI;

public class GameplayHandler : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;

    private Text resultTotal;
    private Text resultRight;
    private Text resultMiss;
    private Text resultWrong;

    #region MONO
    private void Awake()
    {
        resultRight = restartBg.transform.GetChild(2).GetComponent<Text>();
        resultMiss = restartBg.transform.GetChild(3).GetComponent<Text>();
        resultWrong = restartBg.transform.GetChild(4).GetComponent<Text>();
        resultTotal = restartBg.transform.GetChild(5).GetComponent<Text>();
    }

    private void OnEnable()
    {
        GlobalEventManager.GameStateEvent += ChangeGameState;
        GlobalEventManager.WinEvent += ChangeTextToWin;

        Invoke("Restart", 1f);
    }

    private void OnDisable()
    {
        if (IsInvoking("Restart"))
        {
            CancelInvoke("Restart");
        }
        GlobalEventManager.SwitchGameState(false);
    
        GlobalEventManager.WinEvent -= ChangeTextToWin;
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

    private void ChangeTextToWin(int right, int miss, int wrong, int total)
    {
        resultRight.text = "Correct:"+right.ToString();
        resultMiss.text = "Skipped:" + miss.ToString();
        resultWrong.text = "Incorrect:" + wrong.ToString();
        resultTotal.text = "Total points:" + total.ToString();
    }
}
