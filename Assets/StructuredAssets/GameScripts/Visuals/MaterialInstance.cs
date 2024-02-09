using DG.Tweening;
using UnityEngine;

public class MaterialInstance : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    public void Init()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Material target = Instantiate(meshRenderer.material);
        meshRenderer.material = target;
    }

    public void SetColor(Color target, float time = 0.5f)
    {       
        meshRenderer.material.DOColor(target, time);
    }

    public void SetColor(Color target, System.Action callback, string id, float time = 0.5f)
    {
        meshRenderer.material.DOColor(target, time)
            .SetId(id)
            .OnComplete(() =>
            {
                if (callback != null)
                {
                    callback();
                }
            });
    }

}
