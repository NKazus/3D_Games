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

        Restart();
    }

    private void OnDisable()
    {
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
