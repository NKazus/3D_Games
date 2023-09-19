using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayHandler : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private Sprite win;
    [SerializeField] private Sprite lose;

    private Image restartIcon;

    [Inject] private readonly EventManager eventManager;

    #region MONO
    private void Awake()
    {
        restartIcon = restartBg.transform.GetChild(1).GetComponent<Image>();
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
    
        eventManager.WinEvent -= ChangeTextToWin;
        eventManager.GameStateEvent -= ChangeGameState;

        eventManager.SwitchGameState(false);

        restart.onClick.RemoveAllListeners();
        restartBg.SetActive(false);        
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
            restartBg.SetActive(true);
            restart.onClick.AddListener(Restart);
        }
        else
        {            
            restart.onClick.RemoveListener(Restart);
            restartBg.SetActive(false);
            restartIcon.sprite = lose;
            restartIcon.SetNativeSize();
        }
    }

    private void ChangeTextToWin()
    {
        restartIcon.sprite = win;
        restartIcon.SetNativeSize();
    }
}
