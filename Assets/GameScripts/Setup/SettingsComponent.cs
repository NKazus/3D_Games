using UnityEngine;

public class SettingsComponent : MonoBehaviour
{
    [SerializeField] private WandererSystem wanderers;

    private void OnEnable()
    {
        wanderers.SetWanderersPath();
        wanderers.MoveWanderers();
    }

    private void OnDisable()
    {
        wanderers.StopWanderers();
    }
}
