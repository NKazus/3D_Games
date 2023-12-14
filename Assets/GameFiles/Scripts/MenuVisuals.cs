using DG.Tweening;
using UnityEngine;

public class MenuVisuals : MonoBehaviour
{
    [SerializeField] private MenuPlayer[] players;

    private void Awake()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].InitPlayer();
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].MoveWay();
        }
    }

    private void OnDisable()
    {
        DOTween.Kill("menu");
    }
}
