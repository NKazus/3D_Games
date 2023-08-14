using UnityEngine;

public class Cell : MonoBehaviour
{
    private Transform cellTransform;

    private void Awake()
    {
        cellTransform = transform;
    }

    public Vector3 GetCellPosition()
    {
        return cellTransform.position;
    }
}
