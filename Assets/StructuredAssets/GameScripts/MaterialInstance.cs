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

    public void SetColor(Color target)
    {       
        meshRenderer.material.DOColor(target, 0.5f);
    }

}
