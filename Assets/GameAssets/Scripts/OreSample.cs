using DG.Tweening;
using UnityEngine;

public class OreSample : MonoBehaviour
{
    private Transform oreTransform;
    private Vector3 initialScale;

    private bool isHidden;

    public void Init()
    {
        oreTransform = transform;
        initialScale = oreTransform.localScale;
        oreTransform.localScale = Vector3.zero;
        isHidden = true;
    }

    public void Show()
    {
        if (isHidden)
        {
            oreTransform.DOScale(initialScale, 0.5f);
            isHidden = false;
        }
    }

    public void Hide()
    {
        if (!isHidden)
        {
            oreTransform.DOScale(Vector3.zero, 0.5f);
            isHidden = true;
        }
    }
}
