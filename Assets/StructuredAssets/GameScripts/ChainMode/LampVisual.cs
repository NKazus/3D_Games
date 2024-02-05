using UnityEngine;

public class LampVisual : MonoBehaviour
{
    private MaterialInstance statusMat;

    private Color offColor = Color.white;
    private Color singleColor = new Color32(209, 44, 17, 255);
    private Color pairColor = new Color32(202, 157, 41, 255);
    private Color chainColor = new Color32(121, 179, 60, 255);


    public void Init()
    {
        statusMat = transform.GetChild(0).GetComponent<MaterialInstance>();
        statusMat.Init();
    }

    public void UpdateStatus(LampCondition cond)
    {
        switch (cond)
        {
            case LampCondition.Single: statusMat.SetColor(singleColor); break;
            case LampCondition.Pair: statusMat.SetColor(pairColor); break;
            case LampCondition.Chain: statusMat.SetColor(chainColor); break;
            default: statusMat.SetColor(offColor); break;
        }
    }
}
