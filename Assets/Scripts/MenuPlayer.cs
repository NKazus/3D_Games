using DG.Tweening;
using UnityEngine;

public class MenuPlayer : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float time;

    private Transform playerTransform;

    private int iteration;
    private int nextIteration;

    private Vector3[] wayPos;
    private Cell[] cells;

    private void Move()
    {
        if (nextIteration >= wayPoints.Length)
        {
            nextIteration = 0;
        }
        if (iteration >= wayPoints.Length)
        {
            iteration = 0;
        }
        playerTransform.DOMove(wayPos[nextIteration], time)
            .SetId("menu")
            .OnComplete(() => {
                cells[iteration].DoHighlight(false, false);
                cells[nextIteration].DoHighlight(true, false);
                iteration++;
                nextIteration++;
                Move();
            });
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
            cells[i].InitCell();
        }
    }

    public void MoveWay()
    {
        iteration = 0;
        nextIteration = iteration + 1;
        cells[0].DoHighlight(true, false);
        playerTransform.position = wayPos[0];

        Move();
    }
}
