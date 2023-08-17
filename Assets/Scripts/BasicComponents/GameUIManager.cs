using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private Text globalScoreUI;
    [SerializeField] private Text roundsUI;

    [SerializeField] private Image boost;
    [SerializeField] private Image heal;
    [SerializeField] private Image slow;
    [SerializeField] private Image damage;

    public void UpdateValues(int id, int value)
    {
        Text ui;
        string result = value.ToString();
        switch (id)
        {
            case 1: ui = roundsUI; result += "/3"; break;
            default: ui = globalScoreUI; break;
        }

        UpdateText(ui, result);
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f).Play();
    }

    public void UpdateIcons(BuffType id, bool active)
    {
        Image target;
        switch (id)
        {
            case BuffType.Boost: target = boost; break;
            case BuffType.Heal: target = heal; break;
            case BuffType.Slow: target = slow; break;
            default: target = damage; break;
        }
        target.DOFade(active ? 1f : 0.3f, 0.4f);
    }
}
