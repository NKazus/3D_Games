using UnityEngine;

public class ColorInstance : MonoBehaviour
{
    [SerializeField] private Color initialColor;
    private Material objectMaterial;

    private void Awake()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        objectMaterial = Instantiate(meshRenderer.material);
        objectMaterial.color = initialColor;
        meshRenderer.material = objectMaterial;
    }
}
