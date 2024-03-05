using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PanelStates
{
    public GameObject panel;
    public List<bool> states = new List<bool>();
}
public class GameUIController : MonoBehaviour
{
    [SerializeField] private PanelStates[] panels;

    [SerializeField] private GameUITrigger uiManager;

    #region MONO
    private void OnEnable()
    {
        uiManager.ChangeStateEvent += SwitchState;
    }

    private void OnDisable()
    {
        uiManager.ChangeStateEvent -= SwitchState;
    }
    #endregion

    private void SwitchState(GameUI state)
    {
        int currentStateIndex = (int)state;
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].panel.SetActive(panels[i].states[currentStateIndex]);
        }
    }
}
