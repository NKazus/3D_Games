using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameTrigger : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private string winText;
    [SerializeField] private string loseText;

    private Text restartText;
    private Text restartExtra;

    [Inject] private readonly InGameEvents events;

    #region MONO
    private void Awake()
    {
        restartText = restartBg.transform.GetChild(2).GetComponent<Text>();
        restartExtra = restartBg.transform.GetChild(3).GetComponent<Text>();
    }

    private void OnEnable()
    {
        events.SwitchGameEvent += ChangeGameState;
        events.CompleteEvent += UpdateResult;

        Invoke("Restart", 0.5f);
    }

    private void OnDisable()
    {
        if (IsInvoking())
        {
            CancelInvoke("Restart");
        }
        events.SwitchGame(false);

        events.CompleteEvent -= UpdateResult;
        events.SwitchGameEvent -= ChangeGameState;

        restart.gameObject.SetActive(false);
        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }
    #endregion

    private void Restart()
    {
        events.SwitchGame(true);
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
            restartText.text = loseText;
            restartExtra.enabled = false;
            restartBg.SetActive(false);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void UpdateResult(int points)
    {
        restartText.text = winText;
        restartExtra.text = "+ "+ points + " points";
        restartExtra.enabled = true;
    }
}
