using UnityEngine;
using Zenject;

public class ObjectsBond : MonoBehaviour
{
    [SerializeField] private BondColors bondColors;
    [SerializeField] private LineRenderer bondRenderer;
    [SerializeField] private Transform obj1;
    [SerializeField] private Transform obj2;

    [SerializeField] private float maxLength;
    [SerializeField] private float initialLength;

    [Inject] private readonly UpdateManager updateManager;

    private void Awake()
    {
        bondRenderer.positionCount = 5;
    }

    private void OnEnable()
    {
        SwitchBond(true);
    }

    private void Start()
    {
        Vector3 pos1 = obj1.position;
        Vector3 pos2 = obj2.position;

        bondRenderer.SetPosition(0, pos1);
        bondRenderer.SetPosition(1, pos2);

        bondRenderer.colorGradient = bondColors.GetColorGradient();
    }

    private void OnDisable()
    {
        SwitchBond(false);
    }

    private void SwitchBond(bool activate)
    {
        if (activate)
        {
            updateManager.UpdateEvent += LocalUpdate;
        }
        else
        {
            updateManager.UpdateEvent -= LocalUpdate;
        }
    }

    private void LocalUpdate()
    {
        Vector3 startPos = obj1.position;
        Vector3 endPos = obj2.position;

        Vector3 direction = endPos - startPos;

        bondRenderer.SetPosition(0, startPos);
        bondRenderer.SetPosition(1, startPos + 0.2f * direction);
        bondRenderer.SetPosition(2, startPos + 0.5f * direction);
        bondRenderer.SetPosition(3, startPos + 0.8f * direction);
        bondRenderer.SetPosition(4, endPos);

        float currentMagnitude = direction.magnitude;
        bondColors.SetMidAlteredGradient(bondRenderer, (currentMagnitude - initialLength) / (maxLength - initialLength));

        /*if(currentMagnitude > maxLenght)
        {
            Debug.Log("oops");
        }*/
    }
}
