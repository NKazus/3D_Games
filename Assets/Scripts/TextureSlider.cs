using UnityEngine;

public class TextureSlider : MonoBehaviour
{
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color activeColor;
    [SerializeField] private float activeDelta;
    [SerializeField] private float inactiveDelta;

    [SerializeField] private bool init;

    private MeshRenderer meshRenderer;
    private float scrollDelta;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Material materialInstance = Instantiate(meshRenderer.materials[1]);
        meshRenderer.materials[1] = materialInstance;
        SetActive(init);
    }


    private void Update()
    {
        meshRenderer.materials[1].mainTextureOffset = new Vector2(Time.time * scrollDelta, 0);
    }

    public void SetActive(bool active)
    {
        meshRenderer.materials[1].color = active ? activeColor : inactiveColor;
        scrollDelta = active ? activeDelta : inactiveDelta;
    }
}
