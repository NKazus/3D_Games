using UnityEngine;
using UnityEngine.UI;

public class GameplayHandler : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private Sprite win;
    [SerializeField] private Sprite lose;

    private Image restartIcon;

    #region MONO
    private void Awake()
    {
        restartIcon = restartBg.transform.GetChild(1).GetComponent<Image>();
    }

    private void OnEnable()
    {
        GlobalEventManager.GameStateEvent += ChangeGameState;
        GlobalEventManager.WinEvent += ChangeTextToWin;

        Restart();
    }

    private void OnDisable()
    {
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
            restartIcon.sprite = lose;
            restartIcon.SetNativeSize();
            restartBg.SetActive(false);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void ChangeTextToWin()
    {
        restartIcon.sprite = win;
        restartIcon.SetNativeSize();
    }
}
