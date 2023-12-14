using DG.Tweening;
using UnityEngine;
using Zenject;

public class Rotator : MonoBehaviour
{
    [SerializeField] private RotationState state;

    [SerializeField] private Camera menuCam;
    [SerializeField] private Camera gameCam;

    [Inject] private readonly EventManager events;

    private Transform localTransform;
    private Quaternion initialRotation;

    private void Awake()
    {
        localTransform = transform;
        initialRotation = localTransform.rotation;
    }

    private void OnEnable()
    {
        gameCam.enabled = true;
        menuCam.enabled = false;
        events.GameStateEvent += ChangeState;
        ChangeState(true);
    }

    private void OnDisable()
    {
        events.GameStateEvent -= ChangeState;
        menuCam.enabled = true;
        gameCam.enabled = false;        
    }

    private void ChangeState(bool active)
    {
        if(active)
        {
            localTransform.rotation = initialRotation;
            state.Init();
        }
    }

    public void RotateView(bool right)
    {
        Vector3 newState = right ? state.DoNext() : state.DoPrev();
        localTransform.DORotate(newState, 1f);
    }
}
