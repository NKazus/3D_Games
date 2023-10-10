using UnityEngine;
using Zenject;

public class GearButton : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Color buttonColor;

    [Inject] private readonly InGameEvents eventManager;

    private void Awake()
    {
        MeshRenderer buttonRend = GetComponent<MeshRenderer>();
        Material buttonMat = Instantiate(buttonRend.material);
        buttonMat.color = buttonColor;
        buttonRend.material = buttonMat;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Stick"))
        {
            eventManager.PressButton(id);
        }
    }
}
