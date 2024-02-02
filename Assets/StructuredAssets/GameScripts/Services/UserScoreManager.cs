using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UserScoreManager : MonoBehaviour
{
    [SerializeField] private Text luxMedalsUI;
    [SerializeField] private Text ordinaryMedalsUI;
    [SerializeField] private Text poorMedalsUI;

    public void UpdateValues(PlayerRes type, int value)
    {
        Text ui;
        switch (type)
        {
            case PlayerRes.OrdinaryMedal: ui = ordinaryMedalsUI; break;
            case PlayerRes.PoorMedal: ui = poorMedalsUI; break;
            case PlayerRes.LuxMedal: ui = luxMedalsUI; break;
            default: throw new System.NotSupportedException();
        }

        UpdateText(ui, value.ToString());
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.3f);
    }
}
