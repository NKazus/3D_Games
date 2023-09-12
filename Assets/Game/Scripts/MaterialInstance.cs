using UnityEngine;

public class MaterialInstance : MonoBehaviour
{
    [SerializeField] private Color initialColor;
    [SerializeField] private Color alternativeColor;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Material target = Instantiate(meshRenderer.material);
        meshRenderer.material = target;
    }

    private void OnEnable()
    {
        meshRenderer.material.color = initialColor;
    }

    public void Show(bool show, bool active)
    {
        meshRenderer.material.color = active ? alternativeColor : initialColor;
        meshRenderer.enabled = show;
    }
}
