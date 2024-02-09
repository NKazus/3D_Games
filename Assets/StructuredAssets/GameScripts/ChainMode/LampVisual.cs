using UnityEngine;
using Zenject;

public class LampVisual : MonoBehaviour
{
    private MaterialInstance statusMat;

    private Color offColor;
    private Color singleColor;
    private Color pairColor;
    private Color chainColor;

    private Color playerColor;
    private Color botColor;

    [Inject]
    public void SetupLampColors(LampColors lampColors)
    {
        offColor = lampColors.OffColor;
        singleColor = lampColors.SingleColor;
        pairColor = lampColors.PairColor;
        chainColor = lampColors.ChainColor;

        playerColor = lampColors.PlayerColor;
        botColor = lampColors.BotColor;
    }

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
