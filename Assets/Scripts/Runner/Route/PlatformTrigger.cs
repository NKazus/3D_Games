using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField] private TriggerTag triggerTag;

    private BoxCollider triggerCollider;

    private System.Action<TriggerTag> TriggerCallback;

    private void Awake()
    {
        triggerCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Platform") && TriggerCallback!= null)
        {
            TriggerCallback(triggerTag);
        }
    }

    public void SetTriggerCallback(System.Action<TriggerTag> callback)
    {
        TriggerCallback = callback;
    }

    public void SwitchTriggers(bool isOn)
    {
        triggerCollider.enabled = isOn;
    }
}
