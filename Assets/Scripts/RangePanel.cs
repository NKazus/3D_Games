using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RangePanel : MonoBehaviour
{
    [SerializeField] private Image progress;
    [SerializeField] private Transform borderMin;
    [SerializeField] private Transform borderMax;
    [SerializeField] private float borderRangeMin = -380f;
    [SerializeField] private float borderRangeMax = 380f;

    private float range;

    public void InitRange()
    {
        range = borderRangeMax - borderRangeMin;
    }

    public void ResetRange(float min, float max)
    {
        progress.DOFillAmount(0f, 0.5f);

        borderMin.DOLocalMoveX(borderRangeMin + (range * min), 0.3f);
        borderMax.DOLocalMoveX(borderRangeMin + (range * max), 0.3f);
    }

    public void UpdateProgress(float percentage)
    {
        progress.DOFillAmount(percentage, 0.5f);
    }
}
