using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayHandler : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;

    private Text hitText;
    private Text passText;

    [Inject] private readonly EventManager events;

    #region MONO
    private void Awake()
    {
        hitText = restartBg.transform.GetChild(1).GetComponent<Text>();
        passText = restartBg.transform.GetChild(2).GetComponent<Text>();
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
        restart.onClick.RemoveAllListeners();
    }
    #endregion

    private void Restart()
    {
        events.SwitchGameState(true);
    }

    private void ChangeGameState(bool isActive)
    {
        if (!isActive)
        {
            restart.onClick.RemoveAllListeners();
            restartBg.SetActive(true);
            restart.onClick.AddListener(Restart);
        }
        else
        {
            restartBg.SetActive(false);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void ShowResult(int hits, int misses)
    {
        hitText.text = hits.ToString();
        passText.text = misses.ToString();
    }
}
