using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScaleUI : MonoBehaviour
{
    [SerializeField] private Image progressBar;

    public void UpdateValue(float percentage)
    {
        progressBar.DOFillAmount(percentage, 0.3f);
    }

}
