using UnityEngine;

public class TextureTiling : MonoBehaviour
{
    [SerializeField] private float tilingValueX = 10f;
    [SerializeField] private float tilingValueY = 10f;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Material localMaterial = Instantiate(meshRenderer.material);
        meshRenderer.material = localMaterial;
        meshRenderer.material.mainTextureScale = new Vector2(tilingValueX, tilingValueY);
        
    }
}
