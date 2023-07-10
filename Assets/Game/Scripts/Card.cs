using UnityEngine;

[System.Serializable]
public struct Card 
{
    public int id;
    public Sprite image;

    public override bool Equals(object obj)
    {
        return this.id == ((Card) obj).id;
    }
}
