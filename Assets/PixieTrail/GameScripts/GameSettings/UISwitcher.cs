using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PanelStates
{
    public GameObject panel;
    public List<bool> states = new List<bool>();
}
public class UISwitcher : MonoBehaviour
{
    [SerializeField] private PanelStates[] panels;

    [SerializeField] private UIController uiManager;

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

    private void SwitchState(UIPanelsState state)
    {
        int currentStateIndex = (int)state;
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].panel.SetActive(panels[i].states[currentStateIndex]);
        }
    }
}
