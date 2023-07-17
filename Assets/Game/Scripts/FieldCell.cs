using System;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public struct FieldCellIndices
{
    public int cellX;
    public int cellZ;

    public override bool Equals(object obj)
    {
        return cellX == ((FieldCellIndices)obj).cellX && cellZ == ((FieldCellIndices)obj).cellZ;
    }
}
public class FieldCell : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material treasureMaterial;

    [SerializeField] private Color hitColor;
    [SerializeField] private Color nearColor;
    [SerializeField] private Color farColor;

    private FieldCellIndices indices;    

    private Transform cellTransform;
    private bool isScanned = false;
    private bool isTreasure = false;

    private EventTrigger trigger;

    private GameDataHandler dataHandler;

    private MeshRenderer layer0Renderer;
    private GameObject[] layers;

    private GameObject scanLayer;
    private TransparencySwitch scanHandler;    

    private int currentLayer;

    private Func<FieldCellIndices, int> ScanFunction;

    private void Awake()
    {
        cellTransform = transform;
        trigger = GetComponent<EventTrigger>();

        layer0Renderer = cellTransform.GetChild(0).GetComponent<MeshRenderer>();
        layers = new GameObject[3];
        layers[0] = cellTransform.GetChild(1).gameObject;
        layers[1] = cellTransform.GetChild(2).gameObject;
        layers[2] = cellTransform.GetChild(3).gameObject;

        scanLayer = cellTransform.GetChild(4).gameObject;
        scanHandler = scanLayer.GetComponent<TransparencySwitch>();
    }

    private void OnDisable()
    {
        Deactivate();
        if (IsInvoking())
        {
            CancelInvoke("TriggerVictory");
        }
    }

    public void SetIndices(int x, int z, GameDataHandler data)
    {
        indices.cellX = x;
        indices.cellZ = z;

        dataHandler = data;
    }

    private void ClickCell(PointerEventData data)
    {
        switch (dataHandler.ActiveTool)
        {
            case Tool.Scoop:
                if (dataHandler.Scoops > 0 && currentLayer > 0)
                {
                    dataHandler.SetScoops();
                    DigCell();
                }
                break;
            case Tool.Shovel:
                if (dataHandler.Shovels > 0 && currentLayer > 0)
                {
                    dataHandler.RemoveBonus(true);
                    DigCell(true);
                }
                break;
            case Tool.Insight:
                if (dataHandler.Insight > 0)
                {
                    dataHandler.RemoveBonus(false);
                    ScanCell();
                }
                break;
            default: Debug.Log("no such tool found"); break;
        }
        GlobalEventManager.RefreshTools();
    }

    private void ScanCell()
    {
        if(!isScanned)
        {
            int scanValue = ScanFunction(indices);
            Color scanColor;
            switch (scanValue)
            {
                case 0: scanColor = hitColor; break;
                case 1: scanColor = nearColor; break;
                case 2: scanColor = farColor; break;
                default: scanColor = Color.white; break;
            }
            scanHandler.SetColor(scanColor);
            scanLayer.SetActive(true);
            isScanned = true;
        }        
    }

    private void DigCell(bool completely = false)
    {
        int deep = completely ? currentLayer : 1;

        for(int i = 0; i < deep; i++)
        {
            layers[--currentLayer].SetActive(false);
        }

        if (currentLayer <=0 && isTreasure)
        {
            int reward = RandomGenerator.GenerateInt(5, 10);
            GlobalEventManager.DoWin(reward);
            dataHandler.UpdateGlobalScore(reward);
            GlobalEventManager.PlayReward();
            Invoke("TriggerVictory", 2f);
        }
    }

    private void TriggerVictory()
    {
        GlobalEventManager.SwitchGameState(false);
    }

    public Vector3 GetCellPosition()
    {
        return cellTransform.position;
    }

    public void Activate(Func<FieldCellIndices, int> scanRequest)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { ClickCell((PointerEventData)data); });
        trigger.triggers.Add(entry);

        ScanFunction = scanRequest;
    }

    public void Deactivate()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
    }

    public void ResetCell()
    {
        isScanned = false;
        isTreasure = false;

        scanLayer.SetActive(false);
        currentLayer = 3;
        for(int i = 0; i < currentLayer; i++)
        {
            layers[i].SetActive(true);
        }

        layer0Renderer.material = defaultMaterial;
    }

    public void SetTreasure()
    {
        layer0Renderer.material = treasureMaterial;
        isTreasure = true;
    }
}
