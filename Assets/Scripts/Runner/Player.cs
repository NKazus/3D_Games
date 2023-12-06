using UnityEngine;
using FitTheSize.GameServices;

public class Player : MonoBehaviour
{
    [SerializeField] private string wallTag;
    [SerializeField] private string reduceTag;
    [SerializeField] private string increaseTag;
    [SerializeField] private string boostTag;

    private Transform playerTransform;
    private Vector3 initialScale;
    private float scaleSpeed;

    private BoxCollider playerCollider;
    private GameUpdateHandler updateHandler;
    private System.Action<PlayerEvent, bool> CollisionCallback;

    private void Awake()
    {
        playerTransform = transform;
        initialScale = playerTransform.localScale;
        playerCollider = GetComponent<BoxCollider>();
    }

    private void OnDisable()
    {
        ActivateScaling(false);
        ActivateCollisions(false);
    }

    private void Scale()
    {
        Vector3 newScale = new Vector3(playerTransform.localScale.x + scaleSpeed * Time.fixedDeltaTime,
            playerTransform.localScale.y,
            playerTransform.localScale.z + scaleSpeed * Time.fixedDeltaTime);
        playerTransform.localScale = newScale;
    }

    #region COLLISIONS
    private void OnTriggerEnter(Collider other)
    {
        if(CollisionCallback == null)
        {
            return;
        }

        if (other.gameObject.CompareTag(wallTag))
        {
            CollisionCallback(PlayerEvent.Wall, true);
        }
        if (other.gameObject.CompareTag(reduceTag))
        {
            CollisionCallback(PlayerEvent.Reduce, true);
        }
        if (other.gameObject.CompareTag(increaseTag))
        {
            CollisionCallback(PlayerEvent.Increase, true);
        }
        if (other.gameObject.CompareTag(boostTag))
        {
            CollisionCallback(PlayerEvent.Boost, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CollisionCallback == null)
        {
            return;
        }

        if (other.gameObject.CompareTag(reduceTag))
        {
            CollisionCallback(PlayerEvent.Reduce, false);
        }
        if (other.gameObject.CompareTag(increaseTag))
        {
            CollisionCallback(PlayerEvent.Increase, false);
        }
        if (other.gameObject.CompareTag(boostTag))
        {
            CollisionCallback(PlayerEvent.Boost, false);
        }
    }
    #endregion

    public void SetupPlayer(System.Action<PlayerEvent, bool> callback, GameUpdateHandler update)
    {
        Debug.Log("setup player");
        CollisionCallback = callback;
        updateHandler = update;
    }

    public void ActivateScaling(bool activate, float scaleValue = 0f)
    {
        if (activate)
        {
            scaleSpeed = scaleValue;
            updateHandler.GlobalFixedUpdateEvent += Scale;
        }
        else
        {
            updateHandler.GlobalFixedUpdateEvent -= Scale;
        }
    }

    public void ResetPlayer()
    {
        playerTransform.localScale = initialScale;

        //visual movement
    }

    public void ActivateCollisions(bool activate)
    {
        playerCollider.enabled = activate;
    }
}
