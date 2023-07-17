using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum Tool
{
    Scoop = 0,
    Shovel = 1,
    Insight = 2
}

public class FieldManager : MonoBehaviour
{
    [SerializeField] private Button scoopButton;
    [SerializeField] private Button shovelButton;
    [SerializeField] private Button insightButton;
    [SerializeField] private Image activeToolIcon;

    [SerializeField] private Color emptyToolColor;

    [SerializeField] private GameDataHandler dataHandler;
    [SerializeField] private FieldGenerator fieldGenerator;

    [SerializeField] private int treasureCloseRange = 2;

    private bool isScoopEnabled;
    private bool isShovelEnabled;

    private FieldCellIndices treasure;

    private void OnEnable()
    {
        GlobalEventManager.GameStateEvent += ChangeFieldState;

        dataHandler.Refresh();
    }

    private void OnDisable()
    {
        GlobalEventManager.GameStateEvent += ChangeFieldState;
    }

    private void ChangeFieldState(bool activate)
    {
        if (activate)
        {
            fieldGenerator.ResetField();
            dataHandler.SetScoops(true);

            SetTool(scoopButton, Tool.Scoop);
            scoopButton.image.DOColor(Color.white, 0.3f);
            isScoopEnabled = true;

            GenerateTreasure();

            scoopButton.onClick.AddListener(() => { SetTool(scoopButton, Tool.Scoop); });
            shovelButton.onClick.AddListener(() => { SetTool(shovelButton, Tool.Shovel); });
            insightButton.onClick.AddListener(() => { SetTool(insightButton, Tool.Insight); });

            if (dataHandler.Shovels > 0)
            {                
                shovelButton.image.color = Color.white;
                isShovelEnabled = true;
            }
            else
            {
                shovelButton.image.color = emptyToolColor;
                isShovelEnabled = false;
            }

            if (dataHandler.Insight > 0)
            {
                insightButton.image.color = Color.white;
            }
            else
            {
                insightButton.image.color = emptyToolColor;
            }

            ActivateField();
            GlobalEventManager.ToolRefreshEvent += UpdateTools;
        }
        else
        {
            GlobalEventManager.ToolRefreshEvent -= UpdateTools;

            insightButton.onClick.RemoveAllListeners();
            insightButton.onClick.RemoveAllListeners();
            insightButton.onClick.RemoveAllListeners();

            DeactivateField();
        }
    }


    private void ActivateField()
    {
        for (int i = 0; i < fieldGenerator.FieldSize; i++)
        {
            for (int j = 0; j < fieldGenerator.FieldSize; j++)
            {
                fieldGenerator.field[i, j].Activate(ScanTreasure);
            }
        }
    }

    private void DeactivateField()
    {
        for (int i = 0; i < fieldGenerator.FieldSize; i++)
        {
            for (int j = 0; j < fieldGenerator.FieldSize; j++)
            {
                fieldGenerator.field[i, j].Deactivate();
            }
        }
    }

    private void GenerateTreasure()
    {
        treasure.cellX = RandomGenerator.GenerateInt(0, fieldGenerator.FieldSize);
        treasure.cellZ = RandomGenerator.GenerateInt(0, fieldGenerator.FieldSize);

        fieldGenerator.field[treasure.cellX, treasure.cellZ].SetTreasure();
    }

    private int ScanTreasure(FieldCellIndices target)
    {
        if (target.Equals(treasure))
        {
            return 0;
        }

        bool closeRange = (target.cellX - treasure.cellX) * (target.cellX - treasure.cellX)
            + (target.cellZ - treasure.cellZ) * (target.cellZ - treasure.cellZ) < treasureCloseRange * treasureCloseRange;

        if (closeRange)
        {
            return 1;
        }
        else
        {
            return 2;
        }        
    }

    private void SetTool(Button target, Tool value)
    {
        dataHandler.SetActiveTool(value);
        activeToolIcon.sprite = target.transform.GetChild(0).GetComponent<Image>().sprite;
    }

    private void UpdateTools()
    {
        switch (dataHandler.ActiveTool)
        {
            case Tool.Scoop:
                if(dataHandler.Scoops  <= 0)
                {
                    scoopButton.image.DOColor(emptyToolColor, 0.3f);
                    isScoopEnabled = false;
                }
                break;
            case Tool.Shovel:
                if (dataHandler.Shovels <= 0)
                {
                    shovelButton.image.DOColor(emptyToolColor, 0.3f);
                    isShovelEnabled = false;
                }
                break;
            case Tool.Insight:
                if (dataHandler.Insight <= 0)
                {
                    insightButton.image.DOColor(emptyToolColor, 0.3f);
                }
                break;
        }
        if(!isScoopEnabled && !isShovelEnabled)
        {
            GlobalEventManager.SwitchGameState(false);
            GlobalEventManager.PlayVibro();
        }
    }
}
