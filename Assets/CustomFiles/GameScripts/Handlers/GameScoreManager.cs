using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameScoreManager : MonoBehaviour
{
    [SerializeField] private Text mainScoreUI;
    [SerializeField] private Text bonusScoreUI;

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f).Play();
    }

    public void UpdateValues(DataType type, int value)
    {
        Text ui;
        switch (type)
        {
            case DataType.MainScore: ui = mainScoreUI; break;
            case DataType.BonusScore: ui = bonusScoreUI; break;
            default: throw new System.NotSupportedException();
        }

        UpdateText(ui, value.ToString());
    }
}
