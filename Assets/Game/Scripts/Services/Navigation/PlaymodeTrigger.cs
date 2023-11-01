using UnityEngine;
using UnityEngine.UI;
using Zenject;

public enum FinishCondition
{
    Bond,
    Collision,
    Finish
}

public class PlaymodeTrigger : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject finishPanel;
    [SerializeField] private Sprite win;
    [SerializeField] private Sprite lose;
    [SerializeField] private string winText;
    [SerializeField] private string loseText;

    private Image restartIcon;
    private Text restartText;

    [Inject] private readonly GameGlobalEvents globalEvents;

    #region MONO
    private void Awake()
    {
        restartIcon = finishPanel.transform.GetChild(1).GetComponent<Image>();
        restartText = finishPanel.transform.GetChild(2).GetComponent<Text>();
    }

    private void OnEnable()
    {
        restart.onClick.AddListener(Restart);

        globalEvents.EvacuationEvent += ChangeGameState;
        globalEvents.GameFinishEvent += FinishStage;

        Invoke("Restart", 0.5f);
    }

    private void OnDisable()
    {
        globalEvents.SwitchGame(false);
    
        globalEvents.GameFinishEvent -= FinishStage;
        globalEvents.EvacuationEvent -= ChangeGameState;

        finishPanel.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }
    #endregion

    private void Restart()
    {
        globalEvents.SwitchGame(true);
    }

    private void ChangeGameState(bool isActive)
    {
        finishPanel.SetActive(!isActive);
    }

    private void FinishStage(FinishCondition condition)
    {
        Debug.Log("finish:"+condition);
    }
}
