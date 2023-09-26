using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuVisuals : MonoBehaviour
{
    [SerializeField] private Transform[] cells;
    [SerializeField] private Transform player;

    private List<Vector3> jumpPositions;
    private int currentPos;
    private int jumpPoints;

    private Vector3 jumpScale;

    private void Awake()
    {
        jumpPoints = cells.Length;
        jumpPositions = new List<Vector3>();
        for(int i = 0; i < jumpPoints; i++)
        {
            jumpPositions.Add(new Vector3(cells[i].position.x, player.position.y, cells[i].position.z));
        }

        jumpScale = new Vector3(0, 0.1f, 0);
    }

    private void OnEnable()
    {
        currentPos = 0;
        player.position = jumpPositions[currentPos];

        Jump();
    }

    private void OnDisable()
    {
        DOTween.Kill("menu");
    }

    private void Jump()
    {        
        if (++currentPos >= jumpPoints)
        {
            currentPos = 0;
        }
        DOTween.Sequence()
            .SetId("menu")
            .Append(player.DOJump(jumpPositions[currentPos], 0.2f, 1, 0.8f))
            .Join(player.DOShakeScale(0.8f, jumpScale, 5, 90))
            .OnComplete(() => JumpCallback());
    }

    private void JumpCallback()
    {
        Jump();
    }
}
