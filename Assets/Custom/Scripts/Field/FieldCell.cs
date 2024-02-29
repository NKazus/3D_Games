using UnityEngine;
using UnityEngine.EventSystems;

namespace CMGame.Gameplay
{
    [System.Serializable]
    public struct CellIndices
    {
        public int cellX;
        public int cellZ;

        public override bool Equals(object obj)
        {
            return cellX == ((CellIndices)obj).cellX && cellZ == ((CellIndices)obj).cellZ;
        }
    }
    public class FieldCell : MonoBehaviour
    {
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material alternativeMaterial;
        [SerializeField] private Material activeMaterial;

        private CellIndices indices;

        private Transform cellTransform;

        private EventTrigger trigger;
        private MeshRenderer meshRenderer;

        private Material cellMaterial;

        private System.Action<FieldCell> CellCallback;

        private void Awake()
        {
            cellTransform = transform;
            trigger = GetComponent<EventTrigger>();
            meshRenderer = cellTransform.GetChild(0).GetComponent<MeshRenderer>();
            ResetCell();
        }

        private void ClickCell(PointerEventData data)
        {
            if (CellCallback != null)
            {
                CellCallback(this);
            }
        }

        public void SetIndices(int x, int z)
        {
            indices.cellX = x;
            indices.cellZ = z;
        }

        public void SetMaterial(bool alternate)
        {
            cellMaterial = alternate ? alternativeMaterial : defaultMaterial;
            meshRenderer.material = cellMaterial;
        }

        public CellIndices GetIndices()
        {
            return indices;
        }

        public void SetCallback(System.Action<FieldCell> callback)
        {
            CellCallback = callback;
        }

        public Vector3 GetCellPosition()
        {
            return cellTransform.position;
        }

        public void Activate()
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { ClickCell((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }

        public void Deactivate()
        {
            trigger.triggers.RemoveRange(0, trigger.triggers.Count);
        }

        public void ResetCell()
        {
            SwitchCell(false);
        }

        public void SwitchCell(bool active)
        {
            trigger.enabled = active;
            meshRenderer.material = active ? activeMaterial : cellMaterial;
        }
    }
}
