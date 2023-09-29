using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum WallDirection
{
    Top = 0,
    Right = 1,
    Bottom = 2,
    Left = 3
}

public class WallController : MonoBehaviour
{
    [Header("Walls (see tooltip)")]
    [Tooltip("Wall order: Top, Right, Bottom, Left")]
    [SerializeField] private Wall[] walls;

    private WallDirection currentDirection;
    private WallDirection staticDirection;

    private bool isStaticSet;

    [Inject] private readonly RandomGenerator random; 

    private void Awake()
    {
        for(int i = 0; i < walls.Length; i++)
        {
            walls[i].InitWall();
        }
    }

    public void SetStatic(WallDirection dir)
    {
        if (isStaticSet)
        {
            return;
        }
        staticDirection = dir;
        walls[(int)dir].SetStatic();
        walls[(int)dir].Show();
        isStaticSet = true;

        if(currentDirection == dir)//move wall to empty pos
        {
            int currentValue = (int) currentDirection;
            int targetValue;
            do
            {
                targetValue = random.GenerateInt(0, walls.Length);
            }
            while (targetValue == currentValue);
            currentDirection = (WallDirection) targetValue;
            walls[targetValue].Show();
        }
    }

    public void MoveWall(WallDirection dir)
    {
        if(currentDirection == dir || staticDirection == dir)
        {
            return;
        }
        walls[(int)currentDirection].ResetWall();
        currentDirection = dir;
        walls[(int)currentDirection].Show();
    }

    public bool ApplyGravity(WallDirection dir)//true if blocked
    {
        if(currentDirection == dir || staticDirection == dir)
        {
            walls[(int)dir].Blink();
            return true;
        }

        return false;
    }

    public void ResetCurrent()
    {
        currentDirection = WallDirection.Top;
        walls[(int)currentDirection].Show();
    }

    public void ResetAll()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].ResetWall();
        }
        isStaticSet = false;
    }
}
