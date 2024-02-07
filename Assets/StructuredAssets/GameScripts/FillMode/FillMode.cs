using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FillMode : MonoBehaviour
{
    [SerializeField] private FillLampSystem lampSystem;
    [SerializeField] private GameObject noLampPanel;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private Text messageText;
    [SerializeField] private Button hideMessage;

    [Inject] private readonly AppResourceManager resources;

    private void Awake()
    {
        lampSystem.Initialize(HandleLamp);
    }

    private void OnEnable()
    {
        lampSystem.ResetLamps();

        CheckAvailability();
        
        hideMessage.onClick.AddListener(HideMessage);
        messagePanel.SetActive(false);
    }

    private void OnDisable()
    {
        hideMessage.onClick.RemoveListener(HideMessage);
        ChangeFieldState(false);

        if (IsInvoking("PlayEvent"))
        {
            CancelInvoke("PlayEvent");
        }
        if (IsInvoking("FinishTurn"))
        {
            CancelInvoke("FinishTurn");
        }
    }

    private void ChangeFieldState(bool activate)
    {
        if (activate)
        {
            lampSystem.ResetLamps();
            lampSystem.SetChainLamps();
            lampSystem.ActivateLamps();
            lampSystem.SwitchLamps(true);
        }
        else
        {
            lampSystem.DeactivateLamps();
        }
    }

    private void HandleLamp()
    {
        lampSystem.SwitchLamps(false);

        Invoke("PlayBot", 0.5f);
    }

    private void PlayBot()
    {
        lampSystem.PickRandom();

        Invoke("FinishTurn", 0.5f);
    }

    private void FinishTurn()
    {
        if (lampSystem.CheckComplete())
        {
            resources.UpdateRes(PlayerRes.Lamp, -1);
            ChangeFieldState(false);
            hintPanel.SetActive(false);
            if (lampSystem.CalculateVictory())
            {
                resources.UpdateRes(PlayerRes.FreeAction, 1);
                //sound
                messageText.text = "Win";
            }
            else
            {
                //vibro
                messageText.text = "Lose";
            }
            messagePanel.SetActive(true);
            return;
        }

        lampSystem.SwitchLamps(true);
    }

    private void HideMessage()
    {
        messagePanel.SetActive(false);
        CheckAvailability();
    }

    private void CheckAvailability()
    {
        if (!(resources.GetResValue(PlayerRes.Lamp) > 0))
        {
            noLampPanel.SetActive(true);
            hintPanel.SetActive(false);
            return;
        }

        noLampPanel.SetActive(false);
        hintPanel.SetActive(true);
        ChangeFieldState(true);
    }
}
