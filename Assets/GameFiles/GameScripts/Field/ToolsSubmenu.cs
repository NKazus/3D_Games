using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum SubmenuType
{
    Create,
    Destroy
}

public class ToolsSubmenu : MonoBehaviour
{
    [SerializeField] private SubmenuType type;

    private Transform submenuTransform;
    private GameObject submenuGO;

    private void Awake()
    {
        submenuTransform = transform.GetChild(0);
        submenuGO = submenuTransform.gameObject;
    }

    public SubmenuType GetSubmenuType()
    {
        return type;
    }

    public void Reset()
    {
        submenuTransform.localScale = Vector3.zero;
        submenuGO.SetActive(false);
    }

    public void Show()
    {
        submenuGO.SetActive(true);
        submenuTransform.DOScale(new Vector3(1, 1, 1), 0.5f)
            .SetId("submenu");
    }

    public void Hide(System.Action<SubmenuType> callback = null, SubmenuType nextType = SubmenuType.Create)
    {
        submenuTransform.DOScale(Vector3.zero, 0.5f)
            .SetId("submenu")
            .OnComplete(() => {
                if(callback != null)
                {
                    callback(nextType);
                }
            });
    }
}
