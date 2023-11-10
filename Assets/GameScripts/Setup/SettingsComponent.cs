using UnityEngine;

public class SettingsComponent : MonoBehaviour
{
    [SerializeField] private WandererSystem wanderers;

    private void OnEnable()
    {
        Invoke("StartWandering", 0.5f);
    }

    private void OnDisable()
    {
        if (IsInvoking())
        {
            CancelInvoke("StartWandering");
        }
        else
        {
            wanderers.StopWanderers();
        }
    }

    private void StartWandering()
    {
        wanderers.SetWanderersPath();
        wanderers.MoveWanderers();
    }
}
