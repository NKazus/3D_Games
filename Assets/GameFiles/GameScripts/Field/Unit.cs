using DG.Tweening;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitType type;

    private Transform unitTransform;

    private void Awake()
    {
        unitTransform = transform;
    }

    private void OnEnable()
    {
        unitTransform.localScale = Vector3.zero;
    }

    public UnitType GetUnitType()
    {
        return type;
    }

    public void Show(Vector3 targetPos, System.Action callback)
    {
        unitTransform.position = new Vector3(targetPos.x, unitTransform.position.y, targetPos.z);
        unitTransform.DOScale(new Vector3(1, 1, 1), 0.5f)
            .SetId("unit")
            .OnComplete(() => callback());
    }

    public void Hide(System.Action callback, System.Action<Unit> poolCallback)
    {
        unitTransform.DOScale(Vector3.zero, 0.5f)
            .SetId("unit")
            .OnComplete(() => {
                callback();
                poolCallback(this);
            });
    }
}
