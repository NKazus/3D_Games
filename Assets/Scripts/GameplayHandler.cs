using UnityEngine;
using UnityEngine.UI;

public class GameplayHandler : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private Sprite win;
    [SerializeField] private Sprite lose;

    private DotPositionRandomizer dotRandomizer;
    private Image restartText;

    #region MONO
    private void Awake()
    {
        restartText = restartBg.transform.GetChild(0).GetComponent<Image>();
        dotRandomizer = GetComponent<DotPositionRandomizer>();
    }

    private void OnEnable()
    {
        GlobalEventManager.GameStateEvent += ChangeGameState;
        GlobalEventManager.WinEvent += ChangeTextToWin;
        dotRandomizer.InitializeDots();
        GlobalEventManager.SwitchGameState(true);
    }

    private void OnDisable()
    {
        GlobalEventManager.SwitchGameState(false);
        dotRandomizer.ResetDots();
        GlobalEventManager.WinEvent -= ChangeTextToWin;
        GlobalEventManager.GameStateEvent -= ChangeGameState;
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
            restartText.sprite = lose;
            restartText.SetNativeSize();
            restartBg.SetActive(false);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void ChangeTextToWin()
    {
        restartText.sprite = win;
        restartText.SetNativeSize();
    }
}
