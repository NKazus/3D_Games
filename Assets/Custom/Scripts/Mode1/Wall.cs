using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private Material inactiveMaterial;
    [SerializeField] private Material activeMaterial;

    private MeshRenderer meshRenderer;
    private bool isActive;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        ResetWall();
    }

    public void ResetWall()
    {
        meshRenderer.material = inactiveMaterial;
        isActive = false;
    }

    public void Activate()
    {
        isActive = true;
        meshRenderer.material = activeMaterial;
    }

    public bool IsWallActive()
    {
        return isActive;
    }
}
