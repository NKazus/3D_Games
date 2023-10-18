using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Text playerIncomeUI;
    [SerializeField] private Text shieldUI;
    [SerializeField] private Text pollenUI;

    [SerializeField] private Image[] shieldIcons;
    [SerializeField] private Image[] pollenIcons;
    [SerializeField] private Sprite shieldDefault;
    [SerializeField] private Sprite shieldUpgrade;
    [SerializeField] private Sprite pollenDefault;
    [SerializeField] private Sprite pollenUpgrade;

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f).Play();
    }

    public void UpdateIcons(AbilityType id, bool active)
    {
        switch (id)
        {
            case AbilityType.Shield:
                for (int i = 0; i < shieldIcons.Length; i++)
                {
                    shieldIcons[i].sprite = active ? shieldUpgrade : shieldDefault;
                }
                break;
            case AbilityType.Pollen:
                for (int i = 0; i < pollenIcons.Length; i++)
                {
                    pollenIcons[i].sprite = active ? pollenUpgrade : pollenDefault;
                }
                break;
            default: throw new System.NotSupportedException();
        }
    }

    public void UpdateValues(AbilityType id, int value)
    {
        Text ui;
        switch (id)
        {
            case AbilityType.Shield: ui = shieldUI; break;
            case AbilityType.Pollen: ui = pollenUI; break;
            default: throw new System.NotSupportedException();
        }

        UpdateText(ui, value.ToString());
    }
}
