using UnityEngine;

[CreateAssetMenu(fileName = "PixieColors")]
public class PixieColors : ScriptableObject
{
    [SerializeField] private Color[] availableColors;

    public Color[] AllColors => availableColors;
}
