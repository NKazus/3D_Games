using System;
using DG.Tweening;
using UnityEngine;

public class CardHouseElement : MonoBehaviour
{
    private Transform elementTransform;
    private Vector3 elementScale;

    private bool isHidden;

    public void InitElement()
    {
        elementTransform = transform;
        elementScale = elementTransform.localScale;
    }

    public void ShowElement(bool complete)
    {
        elementTransform.DOScale(complete ? elementScale : elementScale * 0.75f, 0.4f)
            .SetId("card_house");
        isHidden = false;
    }

    public void ShowElementCallback(bool complete, Action callback)
    {
        elementTransform.DOScale(complete ? elementScale : elementScale * 0.75f, 0.7f)
            .SetId("menu_card_house")
            .OnComplete(() => callback());
        isHidden = false;
    }

    public void HideElement(bool fast)
    {
        if (isHidden)
        {
            return;
        }

        if (fast)
        {
            elementTransform.localScale = Vector3.zero;
        }
        else
        {
            elementTransform.DOScale(Vector3.zero, 0.3f)
                .SetId("card_house");
        }
        isHidden = true;
    }

    public Vector3 GetPosition()
    {
        return elementTransform.localPosition;
    }
}
