using DG.Tweening;
using UnityEngine;

public class LightSwitcher : MonoBehaviour
{
    [SerializeField] private Light target;
    [SerializeField] private Color[] switchColors;

    int maxIters;
    int currentIter;

    private void Awake()
    {
        maxIters = switchColors.Length;
    }

    private void OnEnable()
    {
        currentIter = 0;
        target.color = switchColors[0];
        Switch();
    }

    private void OnDisable()
    {
        DOTween.Kill("light_switch");
    }

    private void Switch()
    {
        currentIter++;
        if(currentIter >= maxIters)
        {
            currentIter = 0;
        }
        target.DOColor(switchColors[currentIter], 2f)
            .SetId("light_switch")
            .OnComplete(() => Switch());
    }
}
