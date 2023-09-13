using System;
using DG.Tweening;
using UnityEngine;

public class RacePlayer : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;

    private Transform playerTransform;
    private float time;

    private int iteration;
    private Action wayCallback;

    private Vector3[] wayPos;
    private Cell[] cells;

    private void Move()
    {
        if(iteration >= wayPoints.Length)
        {
            wayCallback();
        }
        else
        {
            playerTransform.DOMove(wayPos[iteration++], time)
                .SetId("player")
                .OnComplete(() => { cells[iteration - 1].DoHighlight(true, false);  Move(); });
        }
    }

    public void InitPlayer()
    {
        playerTransform = transform;

        wayPos = new Vector3[wayPoints.Length];
        cells = new Cell[wayPoints.Length];

        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPos[i] = new Vector3(wayPoints[i].position.x, playerTransform.position.y, wayPoints[i].position.z);
            cells[i] = wayPoints[i].GetComponent<Cell>();
        }
    }

    public void ResetPlayer()
    {
        playerTransform.position = wayPos[0];
        for (int i = 1; i < cells.Length; i++)
        {
            cells[i].ResetCell();
        }
        cells[0].DoHighlight(true, false);
    }

    public void SetTime(float moveTime)
    {
        time = moveTime;
    }

    public void MoveDemo(Action callback)
    {
        DOTween.Sequence()
            .SetId("player")
            .Append(playerTransform.DOMove(wayPos[1], time))
            .Append(playerTransform.DOMove(wayPos[0], time))
            .OnComplete(() => callback());
    }

    public void MoveWay(Action callback)
    {
        wayCallback = callback;

        iteration = 1;

        Move();
    }
}
