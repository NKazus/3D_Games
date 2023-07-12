using UnityEngine;

public class TextureShift : MonoBehaviour
{
    [SerializeField] private float speedDeltaX;
    [SerializeField] private float speedDeltaY;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        meshRenderer.material.mainTextureOffset = new Vector2(Mathf.Cos(Time.time) * speedDeltaX, Mathf.Sin(Time.time) * speedDeltaY);
    }
}
