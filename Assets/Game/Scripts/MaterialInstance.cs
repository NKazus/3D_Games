using DG.Tweening;
using UnityEngine;

public class MaterialInstance : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Material target;

    public void Init()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        target = Instantiate(meshRenderer.material);
        meshRenderer.material = target;
    }

    public void ShiftColor(Color targetColor, Color defaultColor)
    {
        DOTween.Sequence()
            .SetId("mat_instance")
            .Append(target.DOColor(targetColor, "_MainColor", 0.1f))
            .Append(target.DOColor(defaultColor, "_MainColor", 0.1f));
    }

    public void SetMaterial(Color targetColor, Texture2D targetTexture)
    {
        target.SetColor("_MainColor", targetColor);
        target.SetTexture("_AlphaTexture", targetTexture);
    }
}
