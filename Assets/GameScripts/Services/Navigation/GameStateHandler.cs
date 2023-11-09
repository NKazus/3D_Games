using System;
using System.Collections.Generic;
using UnityEngine;

namespace MEGame.Navigation
{
    [Serializable]
    public class PanelStates
    {
        public GameObject panel;
        public List<bool> states = new List<bool>();
    }
    public class GameStateHandler : MonoBehaviour
    {
        [SerializeField] private PanelStates[] panels;

        [SerializeField] private GameStateController uiManager;

        #region MONO
        private void OnEnable()
        {
            uiManager.ChangeUIStateEvent += SwitchState;
        }

        private void OnDisable()
        {
            uiManager.ChangeUIStateEvent -= SwitchState;
        }
        #endregion

        private void SwitchState(GameState state)
        {
            int currentStateIndex = (int)state;
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].panel.SetActive(panels[i].states[currentStateIndex]);
            }
        }
    }
}
