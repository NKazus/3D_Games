using DG.Tweening;
using UnityEngine;

public class SlotStatus : MonoBehaviour
{
    private Transform statusTransform;
    private MaterialInstance statusMat;

    private Sequence scaleSequence;
    private bool isPlaying;

    private Color badColor = new Color32(209, 44, 17, 255);
    private Color normalColor = new Color32(202, 157, 41, 255);
    private Color goodColor = new Color32(121, 179, 60, 255);

    private void OnDisable()
    {
        scaleSequence.Rewind();
        isPlaying = false;
    }

    public void Init()
    {
        statusTransform = transform;
        statusMat = statusTransform.GetChild(0).GetComponent<MaterialInstance>();
        statusMat.Init();

        scaleSequence = DOTween.Sequence()
            .SetId("slot_status")
            .Append(statusTransform.DOScale(1.2f, 1f))
            .Append(statusTransform.DOScale(1, 1f))
            .SetLoops(-1);

        isPlaying = false;
    }

    public void ResetStatus()
    {
        statusMat.SetColor(badColor);

        SetPicked(false);
    }

    public void UpdateStatus(SlotCondition cond)
    {
        switch (cond)
        {
            case SlotCondition.Bad: statusMat.SetColor(badColor); break;
            case SlotCondition.Normal: statusMat.SetColor(normalColor); break;
            case SlotCondition.Good: statusMat.SetColor(goodColor); break;
            default: throw new System.NotSupportedException();
        }
    }

    public void SetPicked(bool picked)
    {
        if(picked == isPlaying)
        {
            return;
        }

        if (picked)
        {
            scaleSequence.Play();
            isPlaying = true;
        }
        else
        {
            scaleSequence.Rewind();
            isPlaying = false;
        }
    }
}
