using UnityEngine;

public class BondColors : MonoBehaviour
{
    [SerializeField] private Color edgeColor;
    [SerializeField] private Color blendColor;
    [SerializeField] private Color midColor;

    public Gradient GetColorGradient()
    {
        Gradient tempGradient = new Gradient();

        GradientColorKey[] tempColorKeys = new GradientColorKey[5];
        tempColorKeys[0] = new GradientColorKey(edgeColor, 0f);
        tempColorKeys[1] = new GradientColorKey(blendColor, 0.2f);
        tempColorKeys[2] = new GradientColorKey(midColor, 0.5f);
        tempColorKeys[3] = new GradientColorKey(blendColor, 0.8f);
        tempColorKeys[4] = new GradientColorKey(edgeColor, 1f);

        tempGradient.colorKeys = tempColorKeys;
        return tempGradient;
    }
}
