using UnityEngine;

public class LampVisual : MonoBehaviour
{
    private MaterialInstance statusMat;

    private Color offColor = new Color32(142, 142, 142, 255);
    private Color singleColor = new Color32(195, 74, 120, 255);
    private Color pairColor = new Color32(74, 147, 195, 255);
    private Color chainColor = new Color32(84, 195, 73, 255);

    private Color playerColor = new Color32(181, 148, 68, 255);
    private Color botColor = new Color32(123, 74, 195, 255);

    public void Init()
    {
        statusMat = transform.GetChild(0).GetComponent<MaterialInstance>();
        statusMat.Init();
    }

    public void UpdateStatus(LampCondition cond)
    {
        Color targetColor = cond switch
        {
            LampCondition.Single => singleColor,
            LampCondition.Pair => pairColor,
            LampCondition.Chain => chainColor,
            _ => offColor
        };
        statusMat.SetColor(targetColor);
    }

    public void UpdateStatus(FillCondition cond)
    {
        Color targetColor = cond switch
        {
            FillCondition.Player => playerColor,
            FillCondition.Bot => botColor,
            _ => offColor
        };
        statusMat.SetColor(targetColor);
    }
}
