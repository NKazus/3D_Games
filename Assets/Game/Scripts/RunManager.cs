using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RunManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private HealthBar health;

    [SerializeField] private Route[] routes;
    [SerializeField] private Timer timer;
    [SerializeField] private int maxRounds;

    [SerializeField] private Button startButton;

    private bool finished;
    private int rounds;

    [Inject] private readonly GlobalEventManager events;
    [Inject] private readonly DataHandler dataHandler;

    private void OnEnable()
    {
        events.GameStateEvent += ChangeState;
        dataHandler.UpdateGlobalScore(0);

        startButton.onClick.AddListener(Play);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(Play);

        events.GameStateEvent -= ChangeState;
        DOTween.Kill("jump");
    }

    private void ChangeState(bool activate)
    {
        if (activate)
        {
            routes[0].ResetRoute();
            player.position = routes[0].GetCurrent();
            routes[0].GenerateBuffs();

            //check buffs

            //health and timer events
            //other

            rounds = 0;
            dataHandler.UpdateRounds(rounds);

            startButton.gameObject.SetActive(true);
        }
        else
        {
            timer.Deactivate();
            //events
        }
    }

    private void Play()
    {
        startButton.gameObject.SetActive(false);
        Jump();
        timer.Activate();
    }

    private void Jump()
    {
        Vector3 cellPosition = routes[0].GetNext();
        DOTween.Sequence()
            .SetId("jump")
            .Append(player.DOJump(new Vector3(cellPosition.x, player.position.y, cellPosition.z), 0.2f, 1, 0.4f))
            .Join(player.DOShakeScale(0.4f, new Vector3(0, 0.1f, 0), 5, 90))
            .OnComplete(() => JumpCallback());
    }

    private void JumpCallback()
    {
        if (finished)
        {
            return;
        }
        if (routes[0].IsFinishing())
        {
            rounds++;
            dataHandler.UpdateRounds(rounds);
            if (rounds >= maxRounds)
            {
                dataHandler.RefreshBuffs();
                events.DoWin(1);
                events.SwitchGameState(false);
                return;
            }
        }

        Jump();
    }

    private void CrashPlayer()
    {
        finished = true;
        dataHandler.RefreshBuffs();
        events.SwitchGameState(false);
    }

    private void SetKeyPoints()
    {
        /*pRouteIndex = 0;
        pStart = routes[pRoute].cells[0];
        pFinish = routes[pRoute].cells[routes[pRoute].cells.Count - 1];

        Vector3 cellPosition = fieldGenerator.field[pStart.cellX, pStart.cellZ].GetCellPosition();
        player.position = new Vector3(cellPosition.x, player.position.y, cellPosition.z);
        cellPosition = fieldGenerator.field[pFinish.cellX, pFinish.cellZ].GetCellPosition();
        playerFinish.position = new Vector3(cellPosition.x, playerFinish.position.y, cellPosition.z + finishOffsetZ);

        bRouteIndex = 0;
        bStart = routes[bRoute].cells[0];
        bFinish = routes[bRoute].cells[routes[bRoute].cells.Count - 1];

        cellPosition = fieldGenerator.field[bStart.cellX, bStart.cellZ].GetCellPosition();
        bot.position = new Vector3(cellPosition.x, bot.position.y, cellPosition.z);
        cellPosition = fieldGenerator.field[bFinish.cellX, bFinish.cellZ].GetCellPosition();
        botFinish.position = new Vector3(cellPosition.x, botFinish.position.y, cellPosition.z + finishOffsetZ);*/
    }
}
