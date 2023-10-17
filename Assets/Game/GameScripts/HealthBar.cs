using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private float maxHp;
    [SerializeField] private float hpDelta;

    private float currentHp;

    [Inject] private readonly InGameEvents events;

    public void ResetHp()
    {
        progressBar.fillAmount = 1f;
        currentHp = maxHp;
    }

    public void UpdateHp(float mult)
    {
        currentHp += hpDelta * mult;

        if(currentHp >= maxHp)
        {
            currentHp = maxHp;
        }

        if(currentHp <= 0)
        {
            currentHp = 0;
            events.TriggerFail();
        }
        progressBar.DOFillAmount(currentHp / maxHp, 0.3f);
    }
}
