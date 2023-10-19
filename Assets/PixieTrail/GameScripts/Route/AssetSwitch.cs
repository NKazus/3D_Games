using UnityEngine;

public class AssetSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] assets;

    private int currentAsset;

    public void Switch(int id)
    {
        assets[currentAsset].SetActive(false);
        currentAsset = id;
        assets[currentAsset].SetActive(true);
    }
}
