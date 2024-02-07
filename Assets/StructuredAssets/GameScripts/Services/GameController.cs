using UnityEngine;
using UnityEngine.UI;
using Zenject;

public enum GameResult
{
    Win,
    Lose
}

[System.Serializable]
public class FinishPreset
{
    public GameResult result;
    public string finishText;
    public Sprite icon;
    public int resAmount;
}

public class GameController : MonoBehaviour
{
    [SerializeField] private FinishPreset[] presets;
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;

    private Image restartIcon;
    private Text restartText;

    [Inject] private readonly AppEvents eventManager;
    [Inject] private readonly AppResourceManager resourceManager;

    private void Awake()
    {
        restartIcon = restartBg.transform.GetChild(1).GetComponent<Image>();
        restartText = restartBg.transform.GetChild(2).GetComponent<Text>();
    }

    private void OnEnable()
    {
        resourceManager.Refresh();
        eventManager.GameEvent += ChangeGameState;
        eventManager.FinishEvent += PlayFinish;

        Invoke("Restart", 0.5f);
    }

    private void OnDisable()
    {
        eventManager.DoGame(false);
    
        eventManager.FinishEvent -= PlayFinish;
        eventManager.GameEvent -= ChangeGameState;
  
        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }

    private void Restart()
    {
        eventManager.DoGame(true);
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
            restartBg.SetActive(false);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void PlayFinish(GameResult targetType)
    {
        FinishPreset currentPreset = null;

        for (int j = 0; j < presets.Length; j++)
        {
            if (targetType == presets[j].result)
            {
                currentPreset = presets[j];
                break;
            }
        }

        if(currentPreset == null)
        {
            throw new System.NotSupportedException();
        }

        restartIcon.sprite = currentPreset.icon;
        restartIcon.SetNativeSize();
        restartText.text = currentPreset.finishText;

        resourceManager.UpdateRes(PlayerRes.Lamp, currentPreset.resAmount);
    }
}
