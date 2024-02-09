using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private DOTweenController dtControl;

    [SerializeField] private ChainSwitcher[] switchers;

    private void Awake()
    {
        string id = dtControl.GetId();
        for(int i = 0; i < switchers.Length; i++)
        {
            switchers[i].Init(id);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < switchers.Length; i++)
        {
            switchers[i].StartSwitching();
        }
    }
}
