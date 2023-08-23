using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] private SpaceShip[] ships;

    private void OnEnable()
    {
        for (int i = 0; i < ships.Length; i++)
        {
            ships[i].Init();
            ships[i].MoveShip();
        }
    }
}
