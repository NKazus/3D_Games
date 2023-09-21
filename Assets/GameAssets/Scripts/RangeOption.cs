using UnityEngine;
using UnityEngine.UI;

public class RangeOption : MonoBehaviour
{
    private Material optionMaterial;

    public void InitRange()
    {
        Image rangeImage = GetComponent<Image>();
        optionMaterial = Instantiate(rangeImage.material);
        rangeImage.material = optionMaterial;
    }

    public void SetRange(Color top, Color bot)
    {
        optionMaterial.SetColor("_TopColor", top);
        optionMaterial.SetColor("_BottomColor", bot);
    }

}
