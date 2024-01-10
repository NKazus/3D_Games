using DG.Tweening;
using UnityEngine;

public class Ingredient : MonoBehaviour //setup for scale 1, mesh on the child objects, scene scale = 0
{
    [SerializeField] private int id;

    private Transform localTransform;
    private Vector3 initialPos;
    private Vector3 initialScale = new Vector3 (1, 1, 1);

    private void Awake()
    {
        localTransform = transform;
        initialPos = localTransform.position;
        initialScale = localTransform.localScale;
    }

    private void OnDisable()
    {
        localTransform.localScale = Vector3.zero;
        localTransform.position = initialPos;
    }

    public void SetPosition(Vector3 newPos)
    {
        localTransform.position = newPos;
    }

    public void Rescale(bool up, System.Action callback)
    {
        localTransform.DOScale(up ? initialScale : Vector3.zero, 0.5f)
            .SetId("ingredient")
            .OnComplete(() => callback());
    }

    public int GetId()
    {
        return id;
    }
}
