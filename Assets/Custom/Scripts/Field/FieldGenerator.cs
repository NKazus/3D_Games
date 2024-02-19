using UnityEngine;

namespace CMGame.Gameplay
{
    public class FieldGenerator : MonoBehaviour
    {
        [SerializeField] private Transform fieldParentObject;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private float cellOffset = 0.09f;
        [SerializeField] private Vector3 fieldOffset;

        public FieldCell[,] Generate(int fieldSize)
        {
            FieldCell[,] field = new FieldCell[fieldSize, fieldSize];
            GameObject cell;

            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    cell = Instantiate(cellPrefab);
                    cell.transform.SetParent(fieldParentObject);
                    cell.transform.position =
                        new Vector3(i * cellOffset + fieldOffset.x,
                        transform.position.y + fieldOffset.y, -j * cellOffset + fieldOffset.z);

                    field[i, j] = cell.GetComponent<FieldCell>();
                    field[i, j].SetIndices(i, j);
                }
            }

            return field;
        }
    }
}
