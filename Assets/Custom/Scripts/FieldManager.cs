using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FieldManager : MonoBehaviour
{
    [SerializeField] private Button doubleJumpButton;
    [SerializeField] private Sprite doubleJumpActive;
    [SerializeField] private Sprite doubleJumpInactive;
    [SerializeField] private GameScoreManager scoreManager;

    [SerializeField] private GameDataManager dataHandler;
    [SerializeField] private FieldGenerator fieldGenerator;
    [SerializeField] private Transform player;
    [SerializeField] private Transform finish;
    [SerializeField] private Transform[] coins;
    [SerializeField] private int initialMoves = 15;

    private CellIndices currentCell;
    private CellIndices finishCell;

    [Inject] private readonly GameEvents events;
    [Inject] private readonly RandomValueGenerator generator;

    private int moves;
    

    private void OnEnable()
    {
        events.GameEvent += ChangeFieldState;
        scoreManager.UpdateValues(0, dataHandler.GlobalScore);
        
        scoreManager.UpdateValues(3, dataHandler.DoubleJumps);
        scoreManager.UpdateValues(4, dataHandler.Unlocks);
    }

    private void OnDisable()
    {
        events.GameEvent += ChangeFieldState;
    }

    private void ChangeFieldState(bool activate)
    {
        if (activate)
        {
            doubleJumpButton.image.color = Color.white;
            if(dataHandler.DoubleJumps > 0)
            {
                doubleJumpButton.image.sprite = doubleJumpActive;
            }
            else
            {
                doubleJumpButton.image.sprite = doubleJumpInactive;
            }
            moves = initialMoves;
            scoreManager.UpdateValues(2, moves);
            dataHandler.UpdateCurrentScore(true);

            events.MovePlayerEvent += MovePlayer;
            events.LocksUpdateEvent += UpdateLocks;
            events.CoinEvent += UpdateScore;
        }
        else
        {
            
            events.MovePlayerEvent -= MovePlayer;
            events.LocksUpdateEvent -= UpdateLocks;
            events.CoinEvent -= UpdateScore;
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
        if(Mathf.Abs(currentCell.cellX - cell.cellX) > 1 || Mathf.Abs(currentCell.cellZ - cell.cellZ) > 1)
        {
            dataHandler.RemoveBonus(true);
            scoreManager.UpdateValues(3, dataHandler.DoubleJumps);
            if(dataHandler.DoubleJumps <= 0)
            {
                doubleJumpButton.image.sprite = doubleJumpInactive;
            }
            doubleJumpButton.image.color = Color.white;
        }
        Vector3 cellPosition = Vector3.zero;// fieldGenerator.field[cell.cellX, cell.cellZ].GetCellPosition();
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
            events.DoWin(score);
            dataHandler.UpdateGlobalScore(score);
            scoreManager.UpdateValues(0, dataHandler.GlobalScore);
            events.TriggerGame(false);
            return;
        }
        if(moves <= 0)
        {
            dataHandler.UpdateGlobalScore(-5);
            scoreManager.UpdateValues(0, dataHandler.GlobalScore);
            events.TriggerGame(false);
            return;
        }
    }
}
