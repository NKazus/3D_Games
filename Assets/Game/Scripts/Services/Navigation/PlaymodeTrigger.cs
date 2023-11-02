using UnityEngine;
using UnityEngine.UI;
using Zenject;

public enum FinishCondition
{
    Bond,
    Collision,
    Finish
}

[System.Serializable]
public struct FinishSetup
{
    public FinishCondition conditionValue;
    public string textValue;
    public int rewardValue;
}

public class PlaymodeTrigger : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject finishPanel;
    [SerializeField] private Sprite win;
    [SerializeField] private Sprite lose;
    [SerializeField] private FinishSetup[] presets;

    private Image restartIcon;
    private Text restartText;

    [Inject] private readonly GameGlobalEvents globalEvents;
    [Inject] private readonly GameResourceHandler resources;

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
        FinishSetup currentSetup = presets[0];
        bool presetFound = false;
        for(int i = 0; i < presets.Length; i++)
        {
            if(presets[i].conditionValue == condition)
            {
                currentSetup = presets[i];
                presetFound = true;
            }
        }

        if (!presetFound)
        {
            throw new System.NotImplementedException();
        }

        resources.ChangePointsValue(currentSetup.rewardValue);
        restartIcon.sprite = (currentSetup.conditionValue == FinishCondition.Finish) ? win : lose;
        restartIcon.SetNativeSize();
        restartText.text = currentSetup.textValue;
    }
}
