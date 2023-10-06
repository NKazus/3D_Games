using DG.Tweening;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color alertColor;

    private Material sphereTarget;
    private Material baseTarget;

    private bool isSwitched;

    private void Awake()
    {
        MeshRenderer meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        sphereTarget = Instantiate(meshRenderer.material);
        meshRenderer.material = sphereTarget;

        meshRenderer = transform.GetChild(1).GetComponent<MeshRenderer>();
        baseTarget = Instantiate(meshRenderer.material);
        meshRenderer.material = baseTarget;

        sphereTarget.SetColor("_MainColor", defaultColor);
        baseTarget.SetColor("_MainColor", defaultColor);
        isSwitched = false;
    }

    public void ShiftColor(bool alert)
    {
        if(!alert && !isSwitched)
        {
            return;
        }
        sphereTarget.DOColor(alert ? alertColor : defaultColor, "_MainColor", 0.1f);
        baseTarget.DOColor(alert ? alertColor : defaultColor, "_MainColor", 0.1f);
        isSwitched = alert;
    }
}
