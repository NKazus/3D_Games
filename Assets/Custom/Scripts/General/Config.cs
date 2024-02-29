using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig")]
public class Config : ScriptableObject
{
    [SerializeField] private Color offColor;
    public Color OffColor => offColor;

    [SerializeField] private Color singleColor;
    public Color SingleColor => singleColor;

    [SerializeField] private Color pairColor;
    public Color PairColor => pairColor;

    [SerializeField] private Color chainColor;
    public Color ChainColor => chainColor;

    [SerializeField] private Color playerColor;
    public Color PlayerColor => playerColor;

    [SerializeField] private Color botColor;
    public Color BotColor => botColor;

}
