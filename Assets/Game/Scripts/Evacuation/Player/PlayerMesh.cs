using DG.Tweening;
using UnityEngine;

public class PlayerMesh : MonoBehaviour
{
    private Transform meshTransform;

    private void Awake()
    {
        meshTransform = transform;
    }

    private void OnEnable()
    {
        meshTransform.localScale = new Vector3(1, 1, 1);
    }

    public void Hide(bool restore)
    {
        meshTransform.DOScale(0f, 0.4f)
            .SetId("player")
            .OnComplete(() => {
                if (restore)
                {
                    Show();
                }
            });
    }

    public void Show()
    {
        meshTransform.DOScale(1f, 0.4f)
            .SetId("player");
    }
}
