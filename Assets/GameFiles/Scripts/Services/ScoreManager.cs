using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text chargesUI;
    [SerializeField] private Text moneyUI;


    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f).Play();
    }

    public void UpdateGlobal(bool charges, int value)
    {
        Text target;
        target = charges ? chargesUI : moneyUI;

        UpdateText(target, value.ToString());
    }    

}
