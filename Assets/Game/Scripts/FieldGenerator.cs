using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    [SerializeField] private DataHandler dataHandler;
    [SerializeField] private Transform fieldParentObject;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private float cellOffset = 0.09f;
    [SerializeField] private Vector3 fieldOffset;
    [SerializeField] private static int fieldSize = 7;

    public Cell[,] field;
    public int FieldSize => fieldSize;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        field = new Cell[fieldSize, fieldSize];
        GameObject cell;

        for(int i = 0; i < fieldSize; i++)
        {
            for(int j = 0; j < fieldSize; j++)
            {
                cell = Instantiate(cellPrefab);
                cell.transform.SetParent(fieldParentObject);
                cell.transform.position =
                    new Vector3(i * cellOffset + fieldOffset.x,
                    transform.position.y + fieldOffset.y, -j * cellOffset + fieldOffset.z);
                
                field[i, j] = cell.GetComponent<Cell>();
                field[i, j].SetIndices(i, j, dataHandler);
            }
        }
        
    }

    public void ResetField()
    {
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                field[i, j].ResetCell();
            }
        }
    }
}
