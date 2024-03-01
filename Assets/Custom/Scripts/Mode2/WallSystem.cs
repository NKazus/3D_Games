using UnityEngine;

public class WallSystem : MonoBehaviour
{
    [SerializeField] private SwitchingWall[] borders;
    [SerializeField] private SwitchingWall[] walls;

    public void InitWalls()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].Init();
        }

        for (int i = 0; i < borders.Length; i++)
        {
            borders[i].Init();
        }
    }

    public void ResetWalls()
    {
        for(int i = 0; i < walls.Length; i++)
        {
            walls[i].ResetWall();
        }
    }

    public void ActivateWalls()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].Activate();
        }
    }

    public void DeactivateWalls()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].Deactivate();
        }
    }

    public void SwitchWalls(bool activate)
    {
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].SwitchTrigger(activate);
        }
    }
}
