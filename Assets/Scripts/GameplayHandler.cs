using UnityEngine;
using UnityEngine.UI;

public class GameplayHandler : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private string win;
    [SerializeField] private string lose;

    private Text restartText;
    private Text extraText;

    #region MONO
    private void Awake()
    {
        restartText = restartBg.transform.GetChild(1).GetComponent<Text>();
    }

    private void OnEnable()
    {
        /*GlobalEventManager.GameStateEvent += ChangeGameState;
        GlobalEventManager.WinEvent += ChangeTextToWin;
        dotRandomizer.InitializeDots();
        GlobalEventManager.SwitchGameState(true);*/
    }

    private void OnDisable()
    {
        /*GlobalEventManager.SwitchGameState(false);
        dotRandomizer.ResetDots();
        GlobalEventManager.WinEvent -= ChangeTextToWin;
        GlobalEventManager.GameStateEvent -= ChangeGameState;*/
    }
    #endregion

    private void Restart()
    {
        //GlobalEventManager.SwitchGameState(true);
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
            restartText.text = lose;
            restartBg.SetActive(false);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void ChangeTextToWin()
    {
        restartText.text = win;
    }
}
