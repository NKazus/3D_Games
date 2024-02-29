using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private System.Action<Vector3> InputCallback;

    [Inject] private GameUpdateHandler updateHandler;

    private void OnDisable()
    {
        SwitchInput(false);
    }

    private void CheckHit(Vector3 inputPos)
    {
        Ray ray = mainCamera.ScreenPointToRay(inputPos);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.CompareTag("Floor"))
        {
            if(InputCallback != null)
            {
                InputCallback(hit.point);
            }
        }
    }

    private void LocalUpdate()
    {
#if !UNITY_EDITOR && UNITY_IOS
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CheckHit(Input.GetTouch(0).position);
        }
#elif UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            {
                CheckHit(Input.mousePosition);
            }
        }
#endif
    }

    public void SetInputCallback(System.Action<Vector3> callback)
    {
        InputCallback = callback;
    }

    public void SwitchInput(bool activate)
    {
        if (activate)
        {
            updateHandler.GameUpdateEvent += LocalUpdate;
        }
        else
        {
            updateHandler.GameUpdateEvent -= LocalUpdate;
        }
    }
}
