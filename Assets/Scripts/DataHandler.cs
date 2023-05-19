using UnityEngine;

public class DataHandler : MonoBehaviour
{
    [SerializeField] private int dots = 10;
    public int Dots { get; set; }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("_Dots"))
        {
            Dots = PlayerPrefs.GetInt("_Dots");
        }
        else
        {
            Dots = dots;
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_Dots", Dots);
    }

    public void ResetDots()
    {
        Dots = dots;
    }
}
