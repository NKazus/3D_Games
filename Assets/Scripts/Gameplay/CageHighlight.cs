using UnityEngine;

public enum Highlight
{
    Normal,
    Active,
    Failed
}
public class CageHighlight : MonoBehaviour
{
    [SerializeField] private Color normalColor;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Light target;

    public void SetHighlight(Highlight state)
    {
        Color newColor = state switch
        {
            Highlight.Active => activeColor,
            Highlight.Normal => normalColor,
            Highlight.Failed => failedColor,
            _ => throw new System.NotSupportedException()
        };
        
        target.color = newColor;
    }
}
