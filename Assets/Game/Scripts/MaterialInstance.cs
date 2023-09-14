using UnityEngine;

public class MaterialInstance : MonoBehaviour
{
    [SerializeField] private Color initialColor;
    [SerializeField] private Color alternativeColor;

    private MeshRenderer meshRenderer;

    public void InitMaterial()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Material target = Instantiate(meshRenderer.material);
        meshRenderer.material = target;
    }

    public void Show(bool show, bool active)
    {
        meshRenderer.material.color = active ? alternativeColor : initialColor;
        meshRenderer.enabled = show;
    }
}
