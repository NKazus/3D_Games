using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text treasureUI;
    [SerializeField] private Text sticksUI;

    public void UpdateScore(int value)
    {
        treasureUI.DOText(value.ToString(), 0.5f);
    }

    public void UpdateSticks(int value)
    {
        sticksUI.DOText(value.ToString(), 0.5f);
    }
}
