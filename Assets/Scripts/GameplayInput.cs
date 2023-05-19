using UnityEngine;

public class GameplayInput : MonoBehaviour
{
    [SerializeField] private GlobalUpdateManager updateManager;
    [SerializeField] private RectTransform activationArea;

    private float cameraDepth;
    private Camera mainCamera;

    private void OnEnable()
    {
        mainCamera = Camera.main;
        cameraDepth = -mainCamera.transform.position.z;

        GlobalEventManager.GameStateEvent += ChangeInputState;
        GlobalEventManager.BonusStateEvent += ChangeBonusInputState;

        updateManager.GlobalUpdateEvent += BonusUpdate;
    }

    private void OnDisable()
    {
        updateManager.GlobalUpdateEvent += BonusUpdate;

        GlobalEventManager.BonusStateEvent -= ChangeBonusInputState;
        GlobalEventManager.GameStateEvent -= ChangeInputState;
    }

    private bool ValidateGameplayInput(Vector2 inputPosition)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(activationArea, inputPosition, mainCamera))
        {
            return true;
        }
        return false;
    }


    private void ChangeInputState(bool isActive)
    {
        if (isActive)
        {
            updateManager.GlobalUpdateEvent += LocalUpdate;
        }
        else
        {
            updateManager.GlobalUpdateEvent -= LocalUpdate;
        }
    }

    private void ChangeBonusInputState(bool isActive)
    {
        if (isActive)
        {
            updateManager.GlobalUpdateEvent += BonusUpdate;
        }
        else
        {
            updateManager.GlobalUpdateEvent -= BonusUpdate;
        }
    }

#if UNITY_EDITOR
    private void LocalUpdate()
    {
        if (Input.GetMouseButtonDown(0) && ValidateGameplayInput(Input.mousePosition))
        {
            GlobalEventManager.PushPlayer();
        }
    }
    private void BonusUpdate()
    {
        
        if (Input.GetMouseButtonDown(0) && ValidateGameplayInput(Input.mousePosition))
        {
            Vector3 screenPosition = Input.mousePosition;
            screenPosition.z = cameraDepth;
            GlobalEventManager.MovePlayer(mainCamera.ScreenToWorldPoint(screenPosition));
        }
    }
#endif

#if UNITY_IOS && !UNITY_EDITOR
    private void LocalUpdate()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && ValidateGameplayInput(Input.GetTouch(0).position))
        {
            GlobalEventManager.PushPlayer();
        }
    }
    private void BonusUpdate()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && ValidateGameplayInput(Input.GetTouch(0).position))
        {
            Vector3 screenPosition = Input.GetTouch(0).position;
            screenPosition.z = cameraDepth;
            GlobalEventManager.MovePlayer(mainCamera.ScreenToWorldPoint(screenPosition));
        }
    }
#endif
}
