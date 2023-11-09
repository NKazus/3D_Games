using UnityEngine;
using UnityEngine.EventSystems;

public class GrabButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private System.Action<bool> CustomButtonCallback;

    public void OnPointerDown(PointerEventData data)
    {
        CustomButtonCallback(true);
    }

    public void OnPointerUp(PointerEventData data)
    {
        CustomButtonCallback(false);
    }

    public void SetButtonCallback(System.Action<bool> callback)
    {
        CustomButtonCallback = callback;
    }
}
