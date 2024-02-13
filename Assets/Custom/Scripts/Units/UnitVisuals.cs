using UnityEngine;

public class UnitVisuals : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material activeMaterial;

    private MeshRenderer meshRenderer;

    public void Init()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(bool active)
    {
        meshRenderer.material = active ? activeMaterial : defaultMaterial;
    }
}
