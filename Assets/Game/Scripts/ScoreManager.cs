using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreUI;
    [SerializeField] private Text[] currentScoreUI;


    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }


    private void UpdateText(Text uiText, int value)
    {
        uiText.DOText(value.ToString(), 0.5f);
    }
}
