using System;
using UnityEngine;
using UnityEngine.UI;

public enum AppUIState
{
    Menu = 0,
    Gameplay = 1,
    Exchange = 2,
    Settings = 3,
    Rules = 4
}
public class AppUISwitcher : MonoBehaviour
{
    public event Action<AppUIState> ChangeStateEvent;

    [SerializeField] private Button[] toMenu;
    [SerializeField] private Button toSettings;
    [SerializeField] private Button toFillMode;
    [SerializeField] private Button toChainMode;
    [SerializeField] private Button toRules;
    [SerializeField] private Button toPrivacy;
    [SerializeField] private Button toSupport;

    [SerializeField] private string privacyURL;
    [SerializeField] private string supportURL;

    private AppUIState currentState;

    private void Awake()
    {
       currentState = AppUIState.Menu;
    }

    private void OnEnable()
    {
        for (int i = 0; i < toMenu.Length; i++)
        {
            toMenu[i].onClick.AddListener(() => { TriggerChange(AppUIState.Menu); });
        }
        toSettings.onClick.AddListener(() => { TriggerChange(AppUIState.Settings); });
        toFillMode.onClick.AddListener(() => { TriggerChange(AppUIState.Exchange); });
        toChainMode.onClick.AddListener(() => { TriggerChange(AppUIState.Gameplay); });
        toRules.onClick.AddListener(() => { TriggerChange(AppUIState.Rules); });

        toPrivacy.onClick.AddListener(() => OpenPage(privacyURL));
        toSupport.onClick.AddListener(() => OpenPage(supportURL));
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
        toFillMode.onClick.RemoveAllListeners();
        toChainMode.onClick.RemoveAllListeners();
        toRules.onClick.RemoveAllListeners();

        toPrivacy.onClick.RemoveAllListeners();
        toSupport.onClick.RemoveAllListeners();
    }

    private void TriggerChange(AppUIState state)
    {
        currentState = state;
        ChangeStateEvent?.Invoke(state);
    }

    private void OpenPage(string url)
    {
        Application.OpenURL(url);
    }

}
