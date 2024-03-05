using System.Collections;
using UnityEngine;

public class FloorSystem : MonoBehaviour
{
    [SerializeField] private SwitchingFloor[] floorPanels;

    private IEnumerator FloorController;

    private SwitchingFloor targetPanel;

    private IEnumerator SwitchFloor()
    {
        while (true)
        {
            SwitchTarget();
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
            targetPanel.SwitchFloor(FloorCondition.Warning);
            yield return new WaitForSeconds(1.5f);
            targetPanel.SwitchFloor(FloorCondition.Active);
            yield return new WaitForSeconds(2f);
        }        
    }

    private void SwitchTarget()
    {
        SwitchingFloor temp;
        do
        {
            temp = floorPanels[Random.Range(0, floorPanels.Length)];
        }
        while (temp == targetPanel);

        if(targetPanel != null)
        {
            targetPanel.SwitchFloor(FloorCondition.Inactive);
        }
        targetPanel = temp;
    }

    public void ResetFloor()
    {
        for (int i = 0; i < floorPanels.Length; i++)
        {
            floorPanels[i].SwitchFloor(FloorCondition.Inactive);
        }
    }

    public void InitFloor()
    {
        for (int i = 0; i < floorPanels.Length; i++)
        {
           floorPanels[i].Init();
        }
    }

    public void ActivateFloor()
    {
        FloorController = SwitchFloor();
        StartCoroutine(FloorController);
    }

    public void DeactivateFloor()
    {
        if(FloorController != null)
        {
            StopCoroutine(FloorController);
        }
    }
}
