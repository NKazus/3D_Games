using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform startObject;
    [SerializeField] private float movingSpeed;

    private Transform playerTransform;

    [Inject] private readonly GameUpdateHandler updateHandler;

    private void OnDisable()
    {
        SwitchMovement(false);
    }

    private void LocalUpdate()
    {
        playerTransform.position += new Vector3(0, 0, - movingSpeed * Time.deltaTime);
    }

    public void Init()
    {
        playerTransform = transform;
    }

    public void SwitchMovement(bool activate)
    {
        Debug.Log("player:"+activate);
        if (activate)
        {
            updateHandler.UpdateEvent += LocalUpdate;
        }
        else
        {
            updateHandler.UpdateEvent -= LocalUpdate;
        }
    }

    public void SwitchParent(Transform targetParent)
    {
        playerTransform.parent = targetParent;
    }

    public System.Action<bool> GetMovementCallback()
    {
        return SwitchMovement;
    }

    public void ResetPosition()
    {
        playerTransform.position = startObject.position;
    }
}
