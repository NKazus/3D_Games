using UnityEngine;

public class ChainSwitcher : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private float time;

    private string tweenId;
    private MaterialInstance statusMat;

    private int currentColor;

    public void Init(string id)
    {
        tweenId = id;
        statusMat = transform.GetChild(0).GetComponent<MaterialInstance>();
        statusMat.Init();
    }

    public void StartSwitching()
    {
        currentColor = 0;
        Switch();
    }

    private void Switch()
    {
        currentColor++;
        if(currentColor >= colors.Length)
        {
            currentColor = 0;
        }

        statusMat.SetColor(colors[currentColor], Switch, tweenId, time);
    }
}
