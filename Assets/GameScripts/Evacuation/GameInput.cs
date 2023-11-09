using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameInput : MonoBehaviour
{
    [SerializeField] private GrabButton grab1;
    [SerializeField] private GrabButton grab2;

    private Image button1Image;
    private Image button2Image;

    private void Awake()
    {
        button1Image = grab1.gameObject.GetComponent<Image>();
        button2Image = grab2.gameObject.GetComponent<Image>();
    }

    public void ActivateInput(PlayerID id)
    {
        switch (id)
        {
            case PlayerID.Player1:
                button1Image.DOFade(1f, 0.3f);
                grab1.enabled = true;
                break;
            case PlayerID.Player2:
                button2Image.DOFade(1f, 0.3f);
                grab2.enabled = true;
                break;
            default: throw new System.NotSupportedException();
        }            
    }

    public void DeactivateInput(PlayerID id)
    {
        switch (id)
        {
            case PlayerID.Player1:
                button1Image.DOFade(0.5f, 0.3f);
                grab1.enabled = false;
                break;
            case PlayerID.Player2:
                button2Image.DOFade(0.5f, 0.3f);
                grab2.enabled = false;
                break;
            default: throw new System.NotSupportedException();
        }
    }

    public void LinkInput(PlayerID id, System.Action<bool> callback)
    {
        switch (id)
        {
            case PlayerID.Player1: grab1.SetButtonCallback(callback); break;
            case PlayerID.Player2: grab2.SetButtonCallback(callback); break;
            default: throw new System.NotSupportedException();
        }
    }
}
