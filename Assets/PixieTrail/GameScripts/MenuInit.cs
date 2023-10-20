using UnityEngine;

public class MenuInit : MonoBehaviour
{
    [SerializeField] private MovingObject target;

    private void Awake()
    {
        target.ResetObject(null);
    }

    private void OnEnable()
    {
        target.Move();
    }

    private void OnDisable()
    {
        target.Stop();
    }
}
