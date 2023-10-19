using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private MovingObject player;
    [SerializeField] private MovingObject[] pixies;
    [SerializeField] private Timer timer;

    [SerializeField] private GameObject controlPanel;

    [SerializeField] private ParticleSystem pollenEffect;
    [SerializeField] private MeshRenderer shield;
    [SerializeField] private Material hullMat;
    [SerializeField] private Material shieldMat;

    private MoveButton moveButton;
    private Button shieldButton;
    private Button pollenButton;

    private bool activeState;

    private int currentCollisions;

    [Inject] private readonly InGameResources resources;
    [Inject] private readonly InGameEvents events;
    [Inject] private readonly RandomValueProvider rand;

    private void Awake()
    {
        shieldButton = controlPanel.transform.GetChild(2).GetComponent<Button>();
        pollenButton = controlPanel.transform.GetChild(1).GetComponent<Button>();
    }

    private void OnEnable()
    {
        resources.UpdatePlayerIncome(0);
        resources.UpdateResources();

        events.SwitchGameEvent += Activate;

        SetToolsButtons(true);
    }

    private void Start()
    {
        moveButton = controlPanel.transform.GetChild(0).GetComponent<MoveButton>();
        moveButton.SetButtonCallback(MoveButtonCallback);        

        timer.SetCallback(CalculateTimeout);
    }

    private void OnDisable()
    {
        SetToolsButtons(false);

        events.SwitchGameEvent -= Activate;
        events.PixieCollisionEvent -= CheckCollision;
        timer.Deactivate();

        for (int i = 0; i < pixies.Length; i++)
        {
            pixies[i].Stop();
        }
        player.Stop();        

        DOTween.Kill("pixie_color");
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            resources.ActivateTool(AbilityType.Shield,false);
            resources.ActivateTool(AbilityType.Pollen, false);

            UsePollen(false);
            UseShield(false);

            player.ResetObject(FinishPathCallback);
            for(int i = 0; i < pixies.Length; i++)
            {
                pixies[i].Stop();
                pixies[i].ResetObject(null);
                pixies[i].Move();
            }

            
            currentCollisions = 0;
            events.PixieCollisionEvent += CheckCollision;

            shieldButton.interactable = resources.Shields > 0;
            pollenButton.interactable = resources.Pollen > 0;

            controlPanel.SetActive(true);
            activeState = true;
        }
        else
        {
            timer.Deactivate();
            activeState = false;
            controlPanel.SetActive(false);
            player.Stop();

            events.PixieCollisionEvent -= CheckCollision;
        }
    }

    private void CalculateTimeout()
    {
        Debug.Log("pollen_off");
        resources.ActivateTool(AbilityType.Pollen, false);
        UsePollen(false);

        for (int i = 0; i < pixies.Length; i++)
        {
            pixies[i].UpdateSpeed(resources.CurrentSpeedModifyer);
        }
        pollenButton.interactable = resources.Pollen > 0;
    }

    private void SetToolsButtons(bool active)
    {
        if (active)
        {
            pollenButton.onClick.AddListener(() => UseTool(AbilityType.Pollen));
            shieldButton.onClick.AddListener(() => UseTool(AbilityType.Shield));
        }
        else
        {
            pollenButton.onClick.RemoveAllListeners();
            shieldButton.onClick.RemoveAllListeners();
        }        
    }

    private void UseTool(AbilityType type)
    {
        resources.UpdateResource(type, -1);
        resources.ActivateTool(type, true);

        switch (type)
        {
            case AbilityType.Shield:
                Debug.Log("shield_on");
                shieldButton.interactable = false;
                UseShield(true);
                break;
            case AbilityType.Pollen:
                Debug.Log("pollen_on");
                pollenButton.interactable = false;
                for (int i = 0; i < pixies.Length; i++)
                {
                    pixies[i].UpdateSpeed(resources.CurrentSpeedModifyer);
                }
                UsePollen(true);
                timer.Activate();
                break;
            default: throw new System.NotSupportedException();
        }
    }

    private void UseShield(bool active)
    {
        shield.material = active ? shieldMat : hullMat;
    }

    private void UsePollen(bool active)
    {
        if (active)
        {
            pollenEffect.Play();
        }
        else
        {
            pollenEffect.Stop();
        }
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

    private void CheckCollision()
    {
        Debug.Log("collision");
        currentCollisions++;
        
        if(currentCollisions >= resources.CurrentShieldCharges)
        {
            Debug.Log("lose");

            resources.UpgradeTool(AbilityType.Shield, false);
            resources.UpgradeTool(AbilityType.Pollen, false);
            events.SwitchGame(false);
            return;
        }
        if (currentCollisions >= resources.CurrentShieldCharges - 1)
        {
            Debug.Log("shield_off");
            currentCollisions = 0;
            resources.ActivateTool(AbilityType.Shield, false);
            UseShield(false);
            shieldButton.interactable = resources.Shields > 0;
        }
    }

    private void FinishPathCallback()
    {
        Debug.Log("win");

        resources.UpgradeTool(AbilityType.Shield, false);
        resources.UpgradeTool(AbilityType.Pollen, false);

        int income = rand.GetInt(10, 20);
        resources.UpdatePlayerIncome(income);
        events.CompletePath(income);
        events.SwitchGame(false);
    }
}
