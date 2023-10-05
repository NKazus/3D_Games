using UnityEngine;

public class HighlightController : MonoBehaviour
{
    [SerializeField] private Color[] hightlightColors;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Light target;

    public void ResetHighlight()
    {
        target.color = defaultColor;
    }

    public void UpdateHighlight(int id)
    {
        target.color = hightlightColors[id];
    }
}
