using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text potionsUI;
    [SerializeField] private Text[] reagentsUI;

    private void UpdateText(Text uiText, int value)
    {
        uiText.DOText(value.ToString(), 0.5f);
    }

    public void UpdatePotions(int amount)
    {
        UpdateText(potionsUI, amount);
    }

    public void UpdateReagents(int reagentID, int amount)
    {
        UpdateText(reagentsUI[reagentID], amount);
    }
}
