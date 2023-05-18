using UnityEngine;
using DG.Tweening;

public class Dot : MonoBehaviour
{
    [SerializeField] private float dotSpeed = 1.5f;

    private Transform localTransform;
    private Rigidbody dotRb;
    private SphereCollider dotCollider;
    private Vector3 initialScale;

    private Vector3 targetPosition;

    private void Awake()
    {
        localTransform = transform;
        initialScale = localTransform.localScale;
        dotRb = GetComponent<Rigidbody>();
        dotCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        localTransform.localScale = initialScale;
        GlobalEventManager.HidePlatformsEvent += Hide;
    }

    private void Start()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Target").transform.position;
    }

    private void OnDisable()
    {
        dotCollider.enabled = false;
        dotRb.velocity = Vector2.zero;
        localTransform.position = new Vector3(0, 130, 0);

        GlobalEventManager.HidePlatformsEvent -= Hide;
        DOTween.KillAll(this);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dotCollider.enabled = false;
            GlobalEventManager.ChangeBonusScore(true);
            localTransform.DOScale(Vector3.zero, 0.1f).SetId(this);
        }
        if(collision.gameObject.CompareTag("Target"))
        {
            dotCollider.enabled = false;
            GlobalEventManager.ChangeBonusScore(false);
            DOTween.Sequence()
                .SetId(this)
                .Append(localTransform.DOShakeScale(0.2f))
                .Append(localTransform.DOScale(Vector3.zero, 0.1f));
        }              
    }

    private void Hide()
    {
        PoolManager.PutGameObjectToPool(gameObject);
    }

    public void Setup(Vector3 position)
    {        
        localTransform.position = position;
        Vector3 direction = (targetPosition - localTransform.position).normalized;
        dotRb.AddForce(direction * dotSpeed, ForceMode.VelocityChange);
        dotCollider.enabled = true;
    }
}
