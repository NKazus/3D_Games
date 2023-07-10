using UnityEngine;

public class CardSwitch : MonoBehaviour
{
    [SerializeField] private Sprite image;

    private MeshRenderer cardRenderer;
    private bool init = false;

    private void Awake()
    {
        if (!init)
        {
            Initialize();
            init = true;
        }
    }

    private void Initialize()
    {
        cardRenderer = GetComponent<MeshRenderer>();
        Material cardMaterial = Instantiate(cardRenderer.material);
        cardRenderer.material = cardMaterial;

        if (image != null)
        {
            cardRenderer.material.SetTexture("_MainTex", image.texture);
        }
    }

    public void Switch(Sprite target)
    {
        if (!init)
        {
            Initialize();
            init = true;
        }
        cardRenderer.material.SetTexture("_MainTex", target.texture);
    }
}
