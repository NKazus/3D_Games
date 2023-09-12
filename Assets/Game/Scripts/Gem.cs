using UnityEngine;

public class Gem : MonoBehaviour
{
    private Transform gemTransform;
    private MeshRenderer gemMesh;

    private bool init;

    private void Awake()
    {
        gemTransform = transform;
        gemMesh = GetComponent<MeshRenderer>();
    }

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
        if (!init)
        {
            gemTransform = transform;
            gemMesh = GetComponent<MeshRenderer>();
            init = true;
        }
    }
}
