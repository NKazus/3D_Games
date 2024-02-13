using UnityEngine;
using UnityEngine.UI;
using Zenject;

public enum GamePhase
{
    Switch,
    Play
}

public class GameController : MonoBehaviour
{
    [SerializeField] private FieldCellSystem cellSystem;
    [SerializeField] private UnitSystem unitSystem;

    [SerializeField] private Button startButton;
    [SerializeField] private Button turnButton;

    private int switchCharges = 3;

    [Inject] private readonly GameEvents events;

    private void OnEnable()
    {
        events.GameEvent += ChangeFieldState;

        startButton.onClick.AddListener(StartGame);
        turnButton.onClick.AddListener(EndTurn);
    }

    private void Start()
    {
        //init callbacks
        cellSystem.InitCells();//generate field
        cellSystem.SetCallback(HandleCell);

        unitSystem.InitUnits(cellSystem.GetInitialPlacements(UnitCategory.Player),
            cellSystem.GetInitialPlacements(UnitCategory.Bot));
        unitSystem.SetCallback(HandleUnit);
        unitSystem.SetSwitchCallback(HandleSwitch);
        unitSystem.SetUnitsCallback(HandleFinish);
    }

    private void OnDisable()
    {
        events.GameEvent -= ChangeFieldState;

        startButton.onClick.RemoveListener(StartGame);
        turnButton.onClick.RemoveListener(EndTurn);
    }

    private void ChangeFieldState(bool activate)
    {
        if (activate)
        {
            cellSystem.ActivateCells();//add triggers
            cellSystem.ResetCells();//set all inactive

            unitSystem.ActivateUnits();//triggers
            unitSystem.ResetUnits();
            unitSystem.PlaceUnits();

            if(switchCharges > 0)
            {
                unitSystem.SetPhase(GamePhase.Switch);
                unitSystem.SwitchUnits(true);
            }
            else
            {
                unitSystem.SwitchUnits(false);
            }
        
            startButton.gameObject.SetActive(true);
        }
        else
        {
            turnButton.gameObject.SetActive(false);
            cellSystem.DeactivateCells();//remove triggers
            unitSystem.DeactivateUnits();
        }
    }

    private void StartGame()
    {
        startButton.gameObject.SetActive(false);
        turnButton.gameObject.SetActive(true);

        unitSystem.SetPhase(GamePhase.Play);

        unitSystem.SwitchUnits(true);        
    }

    private void EndTurn()
    {
        //unitSystem.SwitchUnits(false);
        //bot
        HandleBot();
    }

    private void HandleBot()
    {
        unitSystem.RefreshUnits();
        unitSystem.SwitchUnits(true);
    }

    private void HandleCell(FieldCell targetCell)
    {
        unitSystem.MoveUnit(targetCell);
    }

    private void HandleSwitch()
    {
        switchCharges--;
        if(switchCharges <= 0)
        {
            unitSystem.SwitchUnits(false);
        }
    }

    private void HandleUnit(Unit targetUnit)
    {
        //if defence / buff - show submenu
        cellSystem.SwitchCells(targetUnit.GetUnitCell());
    }

    private void HandleFinish(bool win)
    {
        if (win)
        {
            Debug.Log("win");
        }
        else
        {
            Debug.Log("lose");
        }
        events.TriggerGame(false);
    }
}
