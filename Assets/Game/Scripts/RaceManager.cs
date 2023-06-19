using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Route
{
    public List<CellIndices> cells = new List<CellIndices>();
}
public class RaceManager : MonoBehaviour
{
    [SerializeField] private DataHandler dataHandler;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private FieldGenerator fieldGenerator;
    [SerializeField] private Transform player;
    [SerializeField] private Transform bot;
    [SerializeField] private Route[] routes;
    [SerializeField] private Button startButton;
    [SerializeField] private Text buttonText;
    [SerializeField] private Image rewardImage;
    [SerializeField] private Sprite jumpIcon;
    [SerializeField] private Sprite unlockIcon;

    private CellIndices pStart;
    private CellIndices pFinish;
    private CellIndices bStart;
    private CellIndices bFinish;

    private int pRoute;
    private int bRoute;
    private int pRouteIndex;
    private int bRouteIndex;

    private bool refresh;
    private bool finished;
    private bool jumpReward = true;

    private const string START_TEXT = "Start";
    private const string JUMP_TEXT = "Jump";

    private void OnEnable()
    {
        refresh = false;
        Refresh();
        buttonText.text = START_TEXT;
        startButton.onClick.AddListener(Play);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveAllListeners();
        DOTween.KillAll(this);
    }

    private void Refresh()
    {
        DOTween.KillAll(this);
        finished = false;
        jumpReward = !jumpReward;
        rewardImage.sprite = jumpReward ? jumpIcon : unlockIcon;

        fieldGenerator.ResetField();
        PickRoutes();
        SetField();
        SetKeyPoints();
    }

    private void Play()
    {
        startButton.onClick.RemoveListener(Play);
        if (refresh)
        {
            Refresh();
        }
        else
        {
            refresh = true;
        }
        Jump(bot);
        buttonText.text = JUMP_TEXT;
        startButton.onClick.AddListener(() => Jump(player, true));
    }

    private void Jump(Transform obj, bool isPlayer = false)
    {
        if (isPlayer)
        {
            startButton.onClick.RemoveAllListeners();
        }
        CellIndices cell = isPlayer ? routes[pRoute].cells[++pRouteIndex] : routes[bRoute].cells[++bRouteIndex];
        Vector3 cellPosition = fieldGenerator.field[cell.cellX, cell.cellZ].GetCellPosition();
        obj.DOJump(new Vector3(cellPosition.x, obj.position.y, cellPosition.z), 0.4f, 1, isPlayer ? 0.4f : 0.7f)
            .SetId(this)
            .OnComplete(() => JumpCallback(isPlayer));
    }

    private void JumpCallback(bool isPlayer)
    {
        if (finished)
        {
            return;
        }
        if (isPlayer)
        {
            if (routes[pRoute].cells[pRouteIndex].Equals(pFinish))
            {
                finished = true;
                Stop(true);                
                return;
            }
            startButton.onClick.AddListener(() => Jump(player, true));
        }
        else
        {
            if(routes[bRoute].cells[bRouteIndex].Equals(bFinish))
            {
                finished = true;
                Stop(false);                
                return;
            }        
            Jump(bot);
        }
    }

    private void Stop(bool win)
    {
        startButton.onClick.RemoveAllListeners();
        if (win)
        {
            Debug.Log("win");
            dataHandler.AddBonus(jumpReward);
            if (jumpReward)
            {
                scoreManager.UpdateValues(3, dataHandler.DoubleJumps);
            }
            else
            {
                scoreManager.UpdateValues(4, dataHandler.Unlocks);
            }
            GlobalEventManager.PlayMagicDice();
        }
        else
        {
            Debug.Log("lose");
            dataHandler.UpdateGlobalScore(-5);
            scoreManager.UpdateValues(0, dataHandler.GlobalScore);
            GlobalEventManager.PlayVibro();
        }
        buttonText.text = START_TEXT;
        startButton.onClick.AddListener(Play);
    }

    private void PickRoutes()
    {
        pRoute = RandomGenerator.GenerateInt(0, routes.Length);
        do
        {
            bRoute = RandomGenerator.GenerateInt(0, routes.Length);
        }
        while (bRoute == pRoute);
    }

    private void SetKeyPoints()
    {
        pRouteIndex = 0;
        pStart = routes[pRoute].cells[0];
        pFinish = routes[pRoute].cells[routes[pRoute].cells.Count - 1];
        Vector3 cellPosition = fieldGenerator.field[pStart.cellX, pStart.cellZ].GetCellPosition();
        player.position = new Vector3(cellPosition.x, player.position.y, cellPosition.z);

        bRouteIndex = 0;
        bStart = routes[bRoute].cells[0];
        bFinish = routes[bRoute].cells[routes[bRoute].cells.Count - 1];
        cellPosition = fieldGenerator.field[bStart.cellX, bStart.cellZ].GetCellPosition();
        bot.position = new Vector3(cellPosition.x, bot.position.y, cellPosition.z);
    }

    private void SetField()
    {
        for(int i = 0; i < fieldGenerator.FieldSize; i++)
        {
            for(int j = 0; j < fieldGenerator.FieldSize; j++)
            {
                fieldGenerator.field[i, j].Block();
            }
        }

        for(int i = 0; i < routes[bRoute].cells.Count; i++)
        {
            fieldGenerator.field[routes[bRoute].cells[i].cellX, routes[bRoute].cells[i].cellZ].ResetCell();
        }

        for (int i = 0; i < routes[pRoute].cells.Count; i++)
        {
            fieldGenerator.field[routes[pRoute].cells[i].cellX, routes[pRoute].cells[i].cellZ].MarkCell();
        }
    }
}
