using DG.Tweening;
using UnityEngine;

public class TransparencySwitch : MonoBehaviour
{
    [SerializeField] private Color initialColor;
    [SerializeField] private bool init;

    private Transform localTransform;
    private MeshRenderer scanRenderer;
    private Material scanMaterial;

    private float initialScale;

    private Sequence scaleSequence;

    private void Awake()
    {
        localTransform = transform;
        initialScale = localTransform.localScale.x;

        scanRenderer = GetComponent<MeshRenderer>();
        scanMaterial = Instantiate(scanRenderer.material);
        scanRenderer.material = scanMaterial;

        if (init)
        {
            SetColor(initialColor);
        }

        scaleSequence = DOTween.Sequence()
            .Append(localTransform.DOScale(initialScale * 0.7f, 1f))
            .Append(localTransform.DOScale(initialScale, 1f))
            .SetLoops(-1);
    }

    private void OnEnable()
    {
        scaleSequence.Play();
    }

    private void OnDisable()
    {
        scaleSequence.Rewind();
    }

    public void SetColor(Color target)
    {
        scanMaterial.color = target;
    }
}
