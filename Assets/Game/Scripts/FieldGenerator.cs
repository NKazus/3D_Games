using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    [SerializeField] private GameDataHandler dataHandler;
    [SerializeField] private Transform fieldParentObject;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private float cellOffset = 0.07f;
    [SerializeField] private Vector3 fieldOffset;
    [SerializeField] private static int fieldSize = 7;

    public FieldCell[,] field;
    public int FieldSize => fieldSize;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        field = new FieldCell[fieldSize, fieldSize];
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
                
                field[i, j] = cell.GetComponent<FieldCell>();
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
                field[i, j].Deactivate();
            }
        }
    }
}
