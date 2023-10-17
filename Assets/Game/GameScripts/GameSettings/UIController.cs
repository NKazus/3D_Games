using System;
using UnityEngine;
using UnityEngine.UI;

public enum UIPanelsState
{
    Menu = 0,
    Main = 1,
    Store = 2,
    Settings = 3,
    Rules = 4
}
public class UIController : MonoBehaviour
{
    public event Action<UIPanelsState> ChangeStateEvent;

    [SerializeField] private Button[] toMenu;
    [SerializeField] private Button toSettings;
    [SerializeField] private Button toStore;
    [SerializeField] private Button toMain;
    [SerializeField] private Button toRules;
    [SerializeField] private Button toPrivacy;
    [SerializeField] private string privacyURL;

    private UIPanelsState currentState;

    #region MONO
    private void Awake()
    {
       currentState = UIPanelsState.Menu;
    }

    private void OnEnable()
    {
        for (int i = 0; i < toMenu.Length; i++)
        {
            toMenu[i].onClick.AddListener(() => { TriggerNewState(UIPanelsState.Menu); });
        }
        toSettings.onClick.AddListener(() => { TriggerNewState(UIPanelsState.Settings); });
        toStore.onClick.AddListener(() => { TriggerNewState(UIPanelsState.Store); });
        toMain.onClick.AddListener(() => { TriggerNewState(UIPanelsState.Main); });
        toRules.onClick.AddListener(() => { TriggerNewState(UIPanelsState.Rules); });

        toPrivacy.onClick.AddListener(CheckPrivacy);
    }

    private void Start()
    {
        TriggerNewState(currentState);
    }

    private void OnDisable()
    {
        for (int i = 0; i < toMenu.Length; i++)
        {
            toMenu[i].onClick.RemoveAllListeners();
        }
        toSettings.onClick.RemoveAllListeners();
        toStore.onClick.RemoveAllListeners();
        toMain.onClick.RemoveAllListeners();
        toRules.onClick.RemoveAllListeners();

        toPrivacy.onClick.RemoveListener(CheckPrivacy);
    }
    #endregion


    private void TriggerNewState(UIPanelsState state)
    {
        currentState = state;
        ChangeStateEvent?.Invoke(state);
    }

    private void CheckPrivacy()
    {
        Application.OpenURL(privacyURL);
    }

}
