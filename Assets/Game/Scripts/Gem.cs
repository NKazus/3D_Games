using UnityEngine;

public class Gem : MonoBehaviour
{
    private Transform gemTransform;
    private MeshRenderer gemMesh;

    public void ShowGem(bool show)
    {
        gemMesh.enabled = show;
    }

    public void PlaceGem(Vector3 pos)
    {
        gemTransform.position = new Vector3(pos.x, gemTransform.position.y, pos.z);
    }

    public void InitGem()
    {
        gemTransform = transform;
        gemMesh = GetComponent<MeshRenderer>();
    }
}
