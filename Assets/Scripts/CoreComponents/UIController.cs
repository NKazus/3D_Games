using System;
using UnityEngine;
using UnityEngine.UI;

public enum UICondition
{
    Menu = 0,
    Gameplay = 1,
    MagicDice = 2,
    Settings = 3,
    Rules = 4
}
public class UIController : MonoBehaviour
{
    public event Action<UICondition> ChangeStateEvent;

    [SerializeField] private Button[] toMenu;
    [SerializeField] private Button toSettings;
    [SerializeField] private Button toMagicDice;
    [SerializeField] private Button toGameplay;
    [SerializeField] private Button toRules;
    [SerializeField] private Button toPrivacy;
    [SerializeField] private string privacyURL;

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
        toMagicDice.onClick.AddListener(() => { TriggerChange(UICondition.MagicDice); });
        toGameplay.onClick.AddListener(() => { TriggerChange(UICondition.Gameplay); });
        toRules.onClick.AddListener(() => { TriggerChange(UICondition.Rules); });

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
        toMagicDice.onClick.RemoveAllListeners();
        toGameplay.onClick.RemoveAllListeners();
        toRules.onClick.RemoveAllListeners();

        toPrivacy.onClick.RemoveListener(CheckPrivacy);
    }
    #endregion


    private void TriggerChange(UICondition state)
    {
        currentState = state;
        ChangeStateEvent?.Invoke(state);
    }

    private void CheckPrivacy()
    {
        Application.OpenURL(privacyURL);
    }

}
