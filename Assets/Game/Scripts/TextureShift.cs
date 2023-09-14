using UnityEngine;

public class TextureShift : MonoBehaviour
{
    [SerializeField] private float speedDeltaY;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        meshRenderer.material.mainTextureOffset = new Vector2(0f, Time.time * speedDeltaY);
    }
}
