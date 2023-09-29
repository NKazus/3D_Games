using UnityEngine;

[CreateAssetMenu(fileName = "WallParams")]
public class WallParams : ScriptableObject
{
    [SerializeField] private Texture2D staticWall;
    public Texture2D StaticWallTexture => staticWall;

    [SerializeField] private Texture2D movingWall;
    public Texture2D MovingWallTexture => movingWall;

    [SerializeField] private Color staticColor1;
    public Color StaticColorDefault => staticColor1;

    [SerializeField] private Color staticColor2;
    public Color StaticColorActive => staticColor2;

    [SerializeField] private Color movingColor1;
    public Color MovingColorDefault => movingColor1;

    [SerializeField] private Color movingColor2;
    public Color MovingColorActive => movingColor2;
}
