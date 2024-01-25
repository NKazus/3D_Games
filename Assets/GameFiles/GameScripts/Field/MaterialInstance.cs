using UnityEngine;

public class MaterialInstance : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Material target = Instantiate(meshRenderer.material);
        meshRenderer.material = target;
    }

    public void SetColor(Color target)
    {       
        meshRenderer.material.color = target;
    }

}
