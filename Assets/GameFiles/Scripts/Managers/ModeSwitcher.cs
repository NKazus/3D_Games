using UnityEngine;
using UnityEngine.UI;
using Zenject;

public enum EndGameState
{
    NormalWin1,
    NormalWin2,
    AdvancedWin,
    CommonLose
}

[System.Serializable]
public struct EndGameSetup
{
    public EndGameState type;
    public Sprite icon;
    public string info;
}

public class ModeSwitcher : MonoBehaviour//reward from magnet evac diff types
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;

    [SerializeField] private EndGameSetup[] presets;

    private Image restartIcon;
    private Text restartText;
    private Text pointsText;

    [Inject] private readonly EventHandler events;

    private void Awake()
    {
        restartIcon = restartBg.transform.GetChild(1).GetComponent<Image>();
        restartText = restartBg.transform.GetChild(2).GetComponent<Text>();
        pointsText = restartBg.transform.GetChild(3).GetComponent<Text>();
    }

    private void OnEnable()
    {
        events.GameModeEvent += ChangeGameState;
        events.EndGameEvent += HandleEndGame;

        Invoke("Restart", 1f);
        restart.onClick.AddListener(Restart);
    }

    private void OnDisable()
    {
        events.SwitchGameMode(false);

        events.EndGameEvent -= HandleEndGame;
        events.GameModeEvent -= ChangeGameState;

        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }

    private void Restart()
    {
        events.SwitchGameMode(true);
    }

    private void ChangeGameState(bool isActive)
    {
        if (!isActive)
        {
            restartBg.SetActive(true);
        }
        else
        {
            restartBg.SetActive(false);
        }
    }

    private void HandleEndGame(EndGameState state, int points)
    {
        EndGameSetup currentSetup = presets[0];
        bool presetFound = false;
        for (int i = 0; i < presets.Length; i++)
        {
            if (presets[i].type == state)
            {
                currentSetup = presets[i];
                presetFound = true;
            }
        }

        if (!presetFound)
        {
            throw new System.NotImplementedException();
        }

        restartIcon.sprite = currentSetup.icon;
        restartIcon.SetNativeSize();
        restartText.text = currentSetup.info;
        pointsText.text = points.ToString();
    }
}
