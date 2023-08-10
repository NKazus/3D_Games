using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text seedsUI;
    [SerializeField] private Text moneyUI;
    [SerializeField] private Text propsUI;

    private void UpdateText(Text uiText, int value)
    {
        uiText.DOText(value.ToString(), 0.5f);
    }

    public void UpdateSeeds(int amount)
    {
        UpdateText(seedsUI, amount);
    }

    public void UpdateMoney(int amount)
    {
        UpdateText(moneyUI, amount);
    }

    public void UpdateProps(int amount)
    {
        UpdateText(propsUI, amount);
    }
}
