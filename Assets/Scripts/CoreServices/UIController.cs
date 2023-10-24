using System;
using UnityEngine;
using UnityEngine.UI;

public enum UICondition
{
    Menu = 0,
    Gameplay = 1,
    Setup = 2,
    Settings = 3,
    Rules = 4,
    Bonus = 5
}
public class UIController : MonoBehaviour
{
    public event Action<UICondition> ChangeStateEvent;

    [SerializeField] private Button[] toMenu;
    [SerializeField] private Button toSettings;
    [SerializeField] private Button toSetup;
    [SerializeField] private Button toGameplay;
    [SerializeField] private Button toRules;
    [SerializeField] private Button toBonus;

    private UICondition currentState;

    #region MONO
    private void Awake()
    {
       currentState = UICondition.Menu;
    }

    private void OnEnable()
    {
        for (int i = 0; i < toMenu.Length; i++)
        {
            toMenu[i].onClick.AddListener(() => { TriggerChange(UICondition.Menu); });
        }
        toSettings.onClick.AddListener(() => { TriggerChange(UICondition.Settings); });
        toSetup.onClick.AddListener(() => { TriggerChange(UICondition.Setup); });
        toGameplay.onClick.AddListener(() => { TriggerChange(UICondition.Gameplay); });
        toRules.onClick.AddListener(() => { TriggerChange(UICondition.Rules); });
        toBonus.onClick.AddListener(() => { TriggerChange(UICondition.Bonus); });
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
        toSetup.onClick.RemoveAllListeners();
        toGameplay.onClick.RemoveAllListeners();
        toRules.onClick.RemoveAllListeners();
        toBonus.onClick.RemoveAllListeners();
    }
    #endregion


    private void TriggerChange(UICondition state)
    {
        currentState = state;
        ChangeStateEvent?.Invoke(state);
    }
}
