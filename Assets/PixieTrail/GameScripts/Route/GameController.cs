using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private MovingObject player;
    [SerializeField] private MovingObject[] pixies;

    [SerializeField] private GameObject controlPanel;

    private MoveButton moveButton;
    private Button shieldButton;
    private Button pollenButton;

    private bool activeState;

    [Inject] private readonly InGameResources resources;
    [Inject] private readonly InGameEvents events;

    private void OnEnable()
    {
        resources.UpdatePlayerIncome(0);
        resources.UpdateResources();

        events.SwitchGameEvent += Activate;
    }

    private void Start()
    {
        moveButton = controlPanel.transform.GetChild(0).GetComponent<MoveButton>();
        moveButton.SetButtonCallback(MoveButtonCallback);

        shieldButton = controlPanel.transform.GetChild(2).GetComponent<Button>();
        shieldButton = controlPanel.transform.GetChild(1).GetComponent<Button>();
    }

    private void OnDisable()
    {
        events.SwitchGameEvent -= Activate;

        for (int i = 0; i < pixies.Length; i++)
        {
            pixies[i].Stop();
        }
        player.Stop();
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            player.ResetObject(FinishPathCallback);
            for(int i = 0; i < pixies.Length; i++)
            {
                pixies[i].Stop();
                pixies[i].ResetObject(null);
                pixies[i].Move();
            }

            controlPanel.SetActive(true);
            activeState = true;
        }
        else
        {
            activeState = false;
            controlPanel.SetActive(false);

        }
    }

    private void SetToolsButtons(bool active)
    {

    }

    private void MoveButtonCallback(bool isHeld)
    {
        if (!activeState)
        {
            return;
        }

        if (isHeld)
        {
            player.Move();
        }
        else
        {
            player.Stop();
        }
    }

    private void FinishPathCallback()
    {
        events.SwitchGame(false);
    }
}
