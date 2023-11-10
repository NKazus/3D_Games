using System;
using UnityEngine;
using UnityEngine.UI;

namespace MEGame.Navigation
{
    public enum GameState
    {
        Menu = 0,
        Gameplay = 1,
        Shop = 2,
        Settings = 3,
        Rules = 4
    }
    public class GameStateController : MonoBehaviour
    {
        public event Action<GameState> ChangeUIStateEvent;

        [SerializeField] private Button[] toMenu;
        [SerializeField] private Button toSettings;
        [SerializeField] private Button toSetup;
        [SerializeField] private Button toGameplay;
        [SerializeField] private Button toRules;
        [SerializeField] private Button toPrivacy;
        [SerializeField] private string privacyURL;

        private GameState currentState;

        private void Awake()
        {
            currentState = GameState.Menu;
        }

        private void OnEnable()
        {
            for (int i = 0; i < toMenu.Length; i++)
            {
                toMenu[i].onClick.AddListener(() => { TriggerChange(GameState.Menu); });
            }
            toSettings.onClick.AddListener(() => { TriggerChange(GameState.Settings); });
            toSetup.onClick.AddListener(() => { TriggerChange(GameState.Shop); });
            toGameplay.onClick.AddListener(() => { TriggerChange(GameState.Gameplay); });
            toRules.onClick.AddListener(() => { TriggerChange(GameState.Rules); });

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
            toSetup.onClick.RemoveAllListeners();
            toGameplay.onClick.RemoveAllListeners();
            toRules.onClick.RemoveAllListeners();

            toPrivacy.onClick.RemoveListener(CheckPrivacy);
        }

        private void TriggerChange(GameState state)
        {
            currentState = state;
            ChangeUIStateEvent?.Invoke(state);
        }

        private void CheckPrivacy()
        {
            Application.OpenURL(privacyURL);
        }

    }
}
