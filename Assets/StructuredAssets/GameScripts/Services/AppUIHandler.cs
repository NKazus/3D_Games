using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PanelConditions
{
    public GameObject panel;
    public List<bool> states = new List<bool>();
}
public class AppUIHandler : MonoBehaviour
{
    [SerializeField] private PanelConditions[] panels;

    [SerializeField] private AppUISwitcher uiManager;

    private void OnEnable()
    {
        uiManager.ChangeStateEvent += SwitchState;
    }

    private void OnDisable()
    {
        uiManager.ChangeStateEvent -= SwitchState;
    }

    private void SwitchState(AppUIState state)
    {
        int currentStateIndex = (int)state;
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].panel.SetActive(panels[i].states[currentStateIndex]);
        }
    }
}
