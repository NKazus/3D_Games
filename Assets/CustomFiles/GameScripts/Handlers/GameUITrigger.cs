using System;
using UnityEngine;
using UnityEngine.UI;

public enum GameUI
{
    Menu = 0,
    Wall = 1,
    Switch = 2,
    Settings = 3,
    Rules = 4
}
public class GameUITrigger : MonoBehaviour
{
    public event Action<GameUI> ChangeStateEvent;

    [SerializeField] private Button[] toMenu;
    [SerializeField] private Button toSettings;
    [SerializeField] private Button toSwitchMode;
    [SerializeField] private Button toWallMode;
    [SerializeField] private Button toRules;
    [SerializeField] private Button toPrivacy;
    [SerializeField] private string privacyURL;

    private GameUI currentState;

    private void Awake()
    {
       currentState = GameUI.Menu;
    }

    private void OnEnable()
    {
        for (int i = 0; i < toMenu.Length; i++)
        {
            toMenu[i].onClick.AddListener(() => { TriggerChange(GameUI.Menu); });
        }
        toSettings.onClick.AddListener(() => { TriggerChange(GameUI.Settings); });
        toSwitchMode.onClick.AddListener(() => { TriggerChange(GameUI.Switch); });
        toWallMode.onClick.AddListener(() => { TriggerChange(GameUI.Wall); });
        toRules.onClick.AddListener(() => { TriggerChange(GameUI.Rules); });

        toPrivacy.onClick.AddListener(CheckPrivacy);
    }

    private void Start()
    {
        TriggerChange(currentState);
    }

    private void OnDisable()
    {
        for (int i = 0; i < toMenu.Length; i++)
        {
            toMenu[i].onClick.RemoveAllListeners();
        }
        toSettings.onClick.RemoveAllListeners();
        toSwitchMode.onClick.RemoveAllListeners();
        toWallMode.onClick.RemoveAllListeners();
        toRules.onClick.RemoveAllListeners();

        toPrivacy.onClick.RemoveListener(CheckPrivacy);
    }

    private void TriggerChange(GameUI state)
    {
        currentState = state;
        ChangeStateEvent?.Invoke(state);
    }

    private void CheckPrivacy()
    {
        Application.OpenURL(privacyURL);
    }

}
