using UnityEngine;

public class TextureShift : MonoBehaviour
{
    [SerializeField] private float scrollX;
    [SerializeField] private float scrollY;

    private MeshRenderer objRenderer;

    private void Awake()
    {
        objRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        objRenderer.material.mainTextureOffset = new Vector2(scrollX * Mathf.Cos(Time.time), scrollY * Mathf.Sin(Time.time));
    }

}
