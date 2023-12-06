using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using FitTheSize.GameServices;

public abstract class RunnerPlatform : MonoBehaviour
{
    [SerializeField] private BoxCollider[] platformColliders;

    private Transform platformTransform;
    private GameUpdateHandler updateHandler;
    private float platformSpeed;
    private float despawnPosition;

    private System.Action<GameObject> DespawnCallback;

    protected virtual void Awake()
    {
        platformTransform = transform;
    }

    protected void OnDisable()
    {
        SwitchMovement(false);
    }

    protected void Move()
    {
        platformTransform.position = platformTransform.position + new Vector3(0, 0, Time.deltaTime * platformSpeed);

        if(platformTransform.position.z < despawnPosition && DespawnCallback != null)
        {
            DespawnCallback(this.gameObject);
            SwitchMovement(false);
        }
    }

    public abstract void SetupPlatform();

    public void SwitchColliders(bool isOn)
    {
        for (int i = 0; i < platformColliders.Length; i++)
        {
            platformColliders[i].enabled = isOn;
        }
    }

    public void SwitchMovement(bool doMove)
    {
        if (doMove)
        {
            updateHandler.GlobalUpdateEvent += Move;
        }
        else
        {
            updateHandler.GlobalUpdateEvent -= Move;
        }
    }

    public void SetPosition(Vector3 targetPosition)
    {
        platformTransform.position = targetPosition;
    }

    public void SetSpeed(float movementSpeed)
    {
        platformSpeed = movementSpeed;
    }

    public void SetupMovement(GameUpdateHandler update, float despawnZ, System.Action<GameObject> callback)
    {
        updateHandler = update;
        despawnPosition = despawnZ;
        DespawnCallback = callback;
    }
}
