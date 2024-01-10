using System;
using UnityEngine;
using UnityEngine.UI;

public enum PanelState
{
    Menu = 0,
    Gameplay = 1,
    Extra = 2,
    Settings = 3,
    Rules = 4,
    Upgrade = 5
}
public class PanelTrigger : MonoBehaviour
{
    public event Action<PanelState> ChangeStateEvent;

    [SerializeField] private Button[] toMenu;
    [SerializeField] private Button toSettings;
    [SerializeField] private Button toAdvanced;
    [SerializeField] private Button toNormal;
    [SerializeField] private Button toRules;
    [SerializeField] private Button toPrivacy;
    [SerializeField] private Button toDaily;
    [SerializeField] private string privacyURL;

    private PanelState currentState;

    private void Awake()
    {
       currentState = PanelState.Menu;
    }

    private void OnEnable()
    {
        for (int i = 0; i < toMenu.Length; i++)
        {
            toMenu[i].onClick.AddListener(() => { TriggerChange(PanelState.Menu); });
        }
        toSettings.onClick.AddListener(() => { TriggerChange(PanelState.Settings); });
        toAdvanced.onClick.AddListener(() => { TriggerChange(PanelState.Extra); });
        toNormal.onClick.AddListener(() => { TriggerChange(PanelState.Gameplay); });
        toRules.onClick.AddListener(() => { TriggerChange(PanelState.Rules); });
        toDaily.onClick.AddListener(() => { TriggerChange(PanelState.Upgrade); });

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
        toAdvanced.onClick.RemoveAllListeners();
        toNormal.onClick.RemoveAllListeners();
        toRules.onClick.RemoveAllListeners();
        toDaily.onClick.RemoveAllListeners();

        toPrivacy.onClick.RemoveListener(CheckPrivacy);
    }


    private void TriggerChange(PanelState state)
    {
        currentState = state;
        ChangeStateEvent?.Invoke(state);
    }

    private void CheckPrivacy()
    {
        Application.OpenURL(privacyURL);
    }

}
