using UnityEngine;
using Zenject;

public class ObjectsBond : MonoBehaviour
{
    [SerializeField] private BondColors bondColors;
    [SerializeField] private LineRenderer bondRenderer;
    [SerializeField] private Transform obj1;
    [SerializeField] private Transform obj2;

    [SerializeField] private float maxLenght;

    [Inject] private readonly UpdateManager updateManager;
    [Inject] private readonly GameGlobalEvents globalEvents;

    private void Awake()
    {
        bondRenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        globalEvents.GameStateEvent += SwitchBond;
        Debug.Log("bond_on");
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
        globalEvents.GameStateEvent -= SwitchBond;
        Debug.Log("bond_off");
        SwitchBond(false);
    }

    private void SwitchBond(bool activate)
    {
        Debug.Log("update:"+activate);
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
        Vector3 pos1 = obj1.position;
        Vector3 pos2 = obj2.position;

        bondRenderer.SetPosition(0, pos1);
        bondRenderer.SetPosition(1, pos2);

        /*if(Vector3.Distance(pos1, pos2) > maxLenght)
        {
            Debug.Log("oops");
        }*/
    }
}
