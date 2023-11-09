using UnityEngine;

namespace MEGame.Player
{
    public class BondColors : MonoBehaviour
    {
        [SerializeField] private Color edgeColor;
        [SerializeField] private Color blendColor;
        [SerializeField] private Color midColor1;
        [SerializeField] private Color midColor2;

        public Gradient GetColorGradient()
        {
            Gradient tempGradient = new Gradient();

            GradientColorKey[] tempColorKeys = new GradientColorKey[5];
            tempColorKeys[0] = new GradientColorKey(edgeColor, 0f);
            tempColorKeys[1] = new GradientColorKey(blendColor, 0.2f);
            tempColorKeys[2] = new GradientColorKey(midColor1, 0.5f);
            tempColorKeys[3] = new GradientColorKey(blendColor, 0.8f);
            tempColorKeys[4] = new GradientColorKey(edgeColor, 1f);

            GradientAlphaKey[] tempAlphaKeys = new GradientAlphaKey[2];
            tempAlphaKeys[0] = new GradientAlphaKey(1f, 0f);
            tempAlphaKeys[1] = new GradientAlphaKey(1f, 1f);

            tempGradient.mode = GradientMode.Blend;
            tempGradient.SetKeys(tempColorKeys, tempAlphaKeys);
            return tempGradient;
        }

        public void SetMidAlteredGradient(LineRenderer renderer, float lerpValue)
        {
            Gradient tempGradient = renderer.colorGradient;

            GradientColorKey[] tempColorKeys = tempGradient.colorKeys;
            tempColorKeys[tempColorKeys.Length / 2].color = Color.Lerp(midColor1, midColor2, lerpValue);
            tempGradient.colorKeys = tempColorKeys;

            renderer.colorGradient = tempGradient;
        }
    }
}
