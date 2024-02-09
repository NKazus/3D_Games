using UnityEngine;

public class WhirlController : MonoBehaviour
{
    [SerializeField] private Color targetColor;
    [SerializeField] private float targetSpeed;
    [SerializeField] private float targetStrength;

    private Material target;

    private void Awake()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        target = Instantiate(meshRenderer.material);
        meshRenderer.material = target;
    }

    private void Start()
    {
        target.SetColor("_Whirl_Color", targetColor);
        target.SetFloat("_Whirl_Speed", targetSpeed);
        target.SetFloat("_Whirl_Strength", targetStrength);
    }
}
