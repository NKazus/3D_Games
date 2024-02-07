using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UserScoreManager : MonoBehaviour
{
    [SerializeField] private Text lampsUI;
    [SerializeField] private Text freeActionsUI;

    public void UpdateValues(PlayerRes type, int value)
    {
        Text ui;
        switch (type)
        {
            case PlayerRes.Lamp: ui = lampsUI; break;
            case PlayerRes.FreeAction: ui = freeActionsUI; break;
            default: throw new System.NotSupportedException();
        }

        UpdateText(ui, value.ToString());
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.3f);
    }
}
