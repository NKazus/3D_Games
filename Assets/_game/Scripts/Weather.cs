using UnityEngine;

public class Weather : MonoBehaviour
{
    [SerializeField] private Garden garden;

    [SerializeField] private float heatSmoothness;
    [SerializeField] private float heatRotation;
    [SerializeField] private Color heatColor;

    [SerializeField] private float rainSmoothness;    
    [SerializeField] private float rainRotation;    
    [SerializeField] private Color rainColor;

    [SerializeField] private float normalSmoothness;
    [SerializeField] private float normalRotation;
    [SerializeField] private Color normalColor;

    public void DoRain()
    {
        garden.UpdateGroundMaterial(rainSmoothness);
        garden.UpdateLighting(rainRotation, rainColor);
        garden.UpdateRootPlane(GardenState.Rain);
        garden.UpdateRain(true);
    }

    public void DoHeat()
    {
        garden.UpdateGroundMaterial(heatSmoothness);
        garden.UpdateLighting(heatRotation, heatColor);
        garden.UpdateRootPlane(GardenState.Heat);
        garden.UpdateRain(false);
    }

    public void DoNormal()
    {
        garden.UpdateGroundMaterial(normalSmoothness);
        garden.UpdateLighting(normalRotation, normalColor);
        garden.UpdateRootPlane(GardenState.Normal);
        garden.UpdateRain(false);
    }

    public void DoWind()
    {
        garden.UpdatePropState(false);
    }
}
