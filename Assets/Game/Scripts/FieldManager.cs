using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour
{
    [SerializeField] private Button doubleJumpButton;
    [SerializeField] private Sprite doubleJumpActive;
    [SerializeField] private Sprite doubleJumpInactive;
    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private DataHandler dataHandler;
    [SerializeField] private FieldGenerator fieldGenerator;
    [SerializeField] private Transform player;
    [SerializeField] private Transform finish;
    [SerializeField] private float finishOffsetZ = 0.05f;
    [SerializeField] private Transform[] coins;
    [SerializeField] private int initialMoves = 15;
    [SerializeField] private int blocks = 20;

    private CellIndices currentCell;
    private CellIndices finishCell;

    private int jumpValue;
    private bool isDoubleJumpActive;

    private List<CellIndices> blockedCells;
    private List<CellIndices> coinCells;

    private int moves;
    

    private void OnEnable()
    {
        GlobalEventManager.GameStateEvent += ChangeFieldState;
        scoreManager.UpdateValues(0, dataHandler.GlobalScore);
        
        scoreManager.UpdateValues(3, dataHandler.DoubleJumps);
        scoreManager.UpdateValues(4, dataHandler.Unlocks);
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
            SetKeyPoints();
            GenerateLocks();
            GenerateCoins();
            
            isDoubleJumpActive = false;
            jumpValue = 1;
            doubleJumpButton.image.color = Color.white;
            if(dataHandler.DoubleJumps > 0)
            {
                doubleJumpButton.image.sprite = doubleJumpActive;
                doubleJumpButton.onClick.AddListener(ActivateDoubleJump);
            }
            else
            {
                doubleJumpButton.image.sprite = doubleJumpInactive;
            }
            moves = initialMoves;
            scoreManager.UpdateValues(2, moves);
            dataHandler.UpdateCurrentScore(true);
            ActivateCells();
            GlobalEventManager.MovePlayerEvent += MovePlayer;
            GlobalEventManager.LocksUpdateEvent += UpdateLocks;
            GlobalEventManager.CoinEvent += UpdateScore;
        }
        else
        {
            doubleJumpButton.onClick.RemoveListener(ActivateDoubleJump);
            GlobalEventManager.MovePlayerEvent -= MovePlayer;
            GlobalEventManager.LocksUpdateEvent -= UpdateLocks;
            GlobalEventManager.CoinEvent -= UpdateScore;
        }
    }

    private void GenerateLocks()
    {
        blockedCells = new List<CellIndices>();
        blockedCells.Add(currentCell);
        blockedCells.Add(finishCell);
        CellIndices bCell;
        for(int i = 0; i < blocks; i++)
        {
            do
            {
                bCell.cellX = RandomGenerator.GenerateInt(0, fieldGenerator.FieldSize);
                bCell.cellZ = RandomGenerator.GenerateInt(0, fieldGenerator.FieldSize);
            }
            while (blockedCells.Contains(bCell));
            blockedCells.Add(bCell);
        }

        blockedCells.Remove(currentCell);
        blockedCells.Remove(finishCell);
        for (int i = 0; i < blockedCells.Count; i++)
        {
            fieldGenerator.field[blockedCells[i].cellX, blockedCells[i].cellZ].Block();
        }
    }

    private void GenerateCoins()
    {
        coinCells = new List<CellIndices>();
        coinCells.Add(currentCell);
        coinCells.Add(finishCell);
        CellIndices cCell;
        for (int i = 0; i < coins.Length; i++)
        {
            do
            {
                cCell.cellX = RandomGenerator.GenerateInt(0, fieldGenerator.FieldSize);
                cCell.cellZ = RandomGenerator.GenerateInt(0, fieldGenerator.FieldSize);
            }
            while (coinCells.Contains(cCell));
            coinCells.Add(cCell);
        }

        coinCells.Remove(currentCell);
        coinCells.Remove(finishCell);

        Vector3 coinCellPosition;
        for (int i = 0; i < coins.Length; i++)
        {
            coinCellPosition = fieldGenerator.field[coinCells[i].cellX, coinCells[i].cellZ].GetCellPosition();
            coins[i].position = new Vector3(coinCellPosition.x, coins[i].position.y, coinCellPosition.z);
            coins[i].gameObject.SetActive(true);
        }
    }

    private void UpdateLocks()
    {
        scoreManager.UpdateValues(4, dataHandler.Unlocks);
    }

    private void UpdateScore()
    {
        dataHandler.UpdateCurrentScore();
    }

    private void MovePlayer(CellIndices cell)
    {
        DeactivateCells();
        if(Mathf.Abs(currentCell.cellX - cell.cellX) > 1 || Mathf.Abs(currentCell.cellZ - cell.cellZ) > 1)
        {
            dataHandler.RemoveBonus(true);
            scoreManager.UpdateValues(3, dataHandler.DoubleJumps);
            if(dataHandler.DoubleJumps <= 0)
            {
                doubleJumpButton.image.sprite = doubleJumpInactive;
            }
            jumpValue = 1;
            doubleJumpButton.image.color = Color.white;
            isDoubleJumpActive = false;
        }
        Vector3 cellPosition = fieldGenerator.field[cell.cellX, cell.cellZ].GetCellPosition();
        currentCell = cell;
        moves--;
        scoreManager.UpdateValues(2, moves);
        DOTween.Sequence()
            .Append(player.DOJump(new Vector3(cellPosition.x, player.position.y, cellPosition.z), 0.4f, 1, 1f))
            .Join(player.DOShakeScale(1f, new Vector3(0, 0, 1), 5, 90))
            .OnComplete(() => CheckState())
            .Play();
    }

    private void CheckState()
    {
        if(currentCell.Equals(finishCell))
        {
            int score = dataHandler.ScoreValue;
            GlobalEventManager.DoWin(score);
            dataHandler.UpdateGlobalScore(score);
            scoreManager.UpdateValues(0, dataHandler.GlobalScore);
            GlobalEventManager.SwitchGameState(false);
            GlobalEventManager.PlayReward();
            return;
        }
        if(moves <= 0)
        {
            dataHandler.UpdateGlobalScore(-5);
            scoreManager.UpdateValues(0, dataHandler.GlobalScore);
            GlobalEventManager.SwitchGameState(false);
            return;
        }
        ActivateCells();
    }

    private void ActivateDoubleJump()
    {
        if(dataHandler.DoubleJumps > 0)
        {
            isDoubleJumpActive = !isDoubleJumpActive;
            jumpValue = isDoubleJumpActive ? 2 : 1;
            doubleJumpButton.image.color = isDoubleJumpActive ? Color.yellow : Color.white;
            DeactivateCells();
            ActivateCells();
        }
    }

    private void SetKeyPoints()
    {
        finishCell.cellX = RandomGenerator.GenerateInt(0, fieldGenerator.FieldSize);
        finishCell.cellZ = 0;
        Vector3 finishCellPosition = fieldGenerator.field[finishCell.cellX, finishCell.cellZ].GetCellPosition();
        finish.position = new Vector3(finishCellPosition.x, finish.position.y, finishCellPosition.z + finishOffsetZ);

        currentCell.cellX = RandomGenerator.GenerateInt(0, fieldGenerator.FieldSize);
        currentCell.cellZ = 6;
        Vector3 currentCellPosition = fieldGenerator.field[currentCell.cellX, currentCell.cellZ].GetCellPosition();
        player.position = new Vector3(currentCellPosition.x, player.position.y, currentCellPosition.z);
    }

    private void ActivateCells()
    {
        for(int i = 1; i <= jumpValue; i++)
        {
            if(currentCell.cellX + i < fieldGenerator.FieldSize)
            {
                fieldGenerator.field[currentCell.cellX + i, currentCell.cellZ].Activate();
            }
            if (currentCell.cellZ + i < fieldGenerator.FieldSize)
            {
                fieldGenerator.field[currentCell.cellX, currentCell.cellZ + i].Activate();
            }
            if (currentCell.cellX - i >= 0)
            {
                fieldGenerator.field[currentCell.cellX - i, currentCell.cellZ].Activate();
            }
            if (currentCell.cellZ - i >= 0)
            {
                fieldGenerator.field[currentCell.cellX, currentCell.cellZ - i].Activate();
            }
        }
    }

    private void DeactivateCells()
    {
        for (int i = 1; i <= 2; i++)
        {
            if (currentCell.cellX + i < fieldGenerator.FieldSize)
            {
                fieldGenerator.field[currentCell.cellX + i, currentCell.cellZ].Deactivate();
            }
            if (currentCell.cellZ + i < fieldGenerator.FieldSize)
            {
                fieldGenerator.field[currentCell.cellX, currentCell.cellZ + i].Deactivate();
            }
            if (currentCell.cellX - i >= 0)
            {
                fieldGenerator.field[currentCell.cellX - i, currentCell.cellZ].Deactivate();
            }
            if (currentCell.cellZ - i >= 0)
            {
                fieldGenerator.field[currentCell.cellX, currentCell.cellZ - i].Deactivate();
            }
        }
    }
}
