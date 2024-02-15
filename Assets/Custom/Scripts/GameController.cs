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
    [SerializeField] private ActionSubmenu submenu;
    [SerializeField] private BonusAction bonus;
    [SerializeField] private Bot bot;

    [SerializeField] private Button startButton;
    [SerializeField] private Button turnButton;

    private bool isFinished;

    [Inject] private readonly GameEvents events;
    [Inject] private readonly GameDataManager dataManager;

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
        unitSystem.SetDestroyCallback(HandleDestruction);

        submenu.Init(HandleAction);
        bonus.Init(HandleBonus);

        bot.InitBot(unitSystem, cellSystem);
        bot.SetBotCallback(HandleBot);
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
            dataManager.RefreshData();

            cellSystem.ActivateCells();//add triggers
            cellSystem.ResetCells();//set all inactive

            unitSystem.ActivateUnits();//triggers
            unitSystem.ResetUnits();
            unitSystem.PlaceUnits();

            unitSystem.SwitchUnits(UnitCategory.Bot, false);
            if (dataManager.GetData(DataType.Switches) > 0)
            {
                unitSystem.SetPhase(GamePhase.Switch);
                unitSystem.SwitchUnits(UnitCategory.Player, true);
            }
            else
            {
                unitSystem.SwitchUnits(UnitCategory.Player, false);
            }
        
            startButton.gameObject.SetActive(true);
            bonus.ShowBonusMenu(false);
            bonus.ResetBonusMenu(dataManager.GetData(DataType.Adds) > 0);
        }
        else
        {
            bot.KillBot();

            bonus.ShowBonusMenu(false);
            turnButton.gameObject.SetActive(false);
            cellSystem.DeactivateCells();//remove triggers
            unitSystem.DeactivateUnits();
        }
    }

    private void StartGame()
    {
        startButton.gameObject.SetActive(false);
        turnButton.gameObject.SetActive(true);
        bonus.ShowBonusMenu(true);

        unitSystem.SetPhase(GamePhase.Play);
        unitSystem.ResetUnits();
        unitSystem.RefreshUnits(UnitCategory.Player);

        unitSystem.SwitchUnits(UnitCategory.Player, true);
        unitSystem.SwitchUnits(UnitCategory.Bot, true);

        isFinished = false;
    }

    private void EndTurn()
    {
        unitSystem.SwitchUnits(UnitCategory.Player, false);
        cellSystem.ResetTarget();
        unitSystem.ResetTarget();
        submenu.ResetMenu();
        bonus.Deactivate();
     
        unitSystem.RefreshUnits(UnitCategory.Bot);
        bot.StartBot();
    }

    private void HandleBot()
    {
        if (isFinished)
        {
            return;
        }

        unitSystem.RefreshUnits(UnitCategory.Player);
        unitSystem.SwitchUnits(UnitCategory.Player, true);
        unitSystem.SwitchUnits(UnitCategory.Bot, true);

        bonus.ResetBonusMenu(dataManager.GetData(DataType.Adds) > 0);
    }

    private void HandleCell(FieldCell targetCell)
    {
        unitSystem.MoveUnit(targetCell);
        submenu.ResetMenu();
        bonus.Deactivate();
    }

    private void HandleSwitch()
    {
        dataManager.UpdateData(DataType.Switches, -1);
        if(dataManager.GetData(DataType.Switches) <= 0)
        {
            unitSystem.SwitchUnits(UnitCategory.Player, false);
        }
    }

    private void HandleUnit(Unit targetUnit)
    {
        bonus.SwitchButton();
        submenu.SwitchMenu(targetUnit);
        cellSystem.SwitchCells(targetUnit.GetUnitCell());
    }

    private void HandleAction()
    {
        unitSystem.PerformAction();
    }

    private void HandleBonus()
    {
        unitSystem.AddActions();
        dataManager.UpdateData(DataType.Adds, -1);
        bonus.ResetBonusMenu(false);
    }

    private void HandleDestruction(FieldCell cell)
    {
        cellSystem.FreeCell(cell);
    }

    private void HandleFinish(bool win)
    {
        isFinished = true;

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
