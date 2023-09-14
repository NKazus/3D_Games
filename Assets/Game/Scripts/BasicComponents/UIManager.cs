using System;
using UnityEngine;
using UnityEngine.UI;

public enum GameUIState
{
    Menu = 0,
    Gameplay = 1,
    MagicDice = 2,
    Settings = 3,
    Rules = 4
}
public class UIManager : MonoBehaviour
{
    public event Action<GameUIState> ChangeStateEvent;

    [SerializeField] private Button[] toMenu;
    [SerializeField] private Button toSettings;
    [SerializeField] private Button toMagicDice;
    [SerializeField] private Button toGameplay;
    [SerializeField] private Button toRules;
    [SerializeField] private Button toPrivacy;
    [SerializeField] private string privacyURL;

    private GameUIState currentState;

    #region MONO
    private void Awake()
    {
       currentState = GameUIState.Menu;
    }

    private void OnEnable()
    {
        for (int i = 0; i < toMenu.Length; i++)
        {
            toMenu[i].onClick.AddListener(() => { TriggerChange(GameUIState.Menu); });
        }
        toSettings.onClick.AddListener(() => { TriggerChange(GameUIState.Settings); });
        toMagicDice.onClick.AddListener(() => { TriggerChange(GameUIState.MagicDice); });
        toGameplay.onClick.AddListener(() => { TriggerChange(GameUIState.Gameplay); });
        toRules.onClick.AddListener(() => { TriggerChange(GameUIState.Rules); });

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


    private void TriggerChange(GameUIState state)
    {
        currentState = state;
        ChangeStateEvent?.Invoke(state);
    }

    private void CheckPrivacy()
    {
        Application.OpenURL(privacyURL);
    }

}
