using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameplayHandler : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private Sprite win;
    [SerializeField] private string winText;
    [SerializeField] private Sprite lose;
    [SerializeField] private string loseText;

    private Image restartIcon;
    private Text restartText;

    #region MONO
    private void Awake()
    {
        restartIcon = restartBg.transform.GetChild(1).GetComponent<Image>();
        restartText = restartBg.transform.GetChild(3).GetComponent<Text>();
    }

    private void OnEnable()
    {
        GlobalEventManager.GameStateEvent += ChangeGameState;
        GlobalEventManager.WinEvent += ChangeTextToWin;

        Invoke("Restart", 1f);
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
            restartBg.SetActive(true);
            restartBg.transform.DOScale(new Vector3(1, 1, 1), 0.5f).OnComplete(() => restart.gameObject.SetActive(true));
            restart.onClick.AddListener(Restart);
        }
        else
        {
            restart.gameObject.SetActive(false);
            restartIcon.sprite = lose;
            restartIcon.SetNativeSize();
            restartText.text = loseText;
            restartBg.transform.DOScale(Vector3.zero, 0.5f);
            restartBg.SetActive(false);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void ChangeTextToWin()
    {
        restartIcon.sprite = win;
        restartIcon.SetNativeSize();
        restartText.text = winText;
    }
}
