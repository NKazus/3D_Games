using UnityEngine;

public class MaterialInstance : MonoBehaviour
{
    [SerializeField] private Color initialColor;
    [SerializeField] private Color alternativeColor;

    private MeshRenderer meshRenderer;
    private bool isChanged;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Material target = Instantiate(meshRenderer.material);
        meshRenderer.material = target;
    }

    private void OnEnable()
    {
        meshRenderer.material.color = initialColor;
        isChanged = false;
    }

    public bool ChangeColor()
    {
        bool checkState = isChanged;
        if (!isChanged)
        {
            meshRenderer.material.color = alternativeColor;
            isChanged = true;
        }
        return checkState;
    }

    public void ResetColor()
    {
        meshRenderer.material.color = initialColor;
        isChanged = false;
    }
}
