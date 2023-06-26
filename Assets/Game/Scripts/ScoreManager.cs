using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text globalScoreUI;
    [SerializeField] private Text movesUI;
    [SerializeField] private Text jumpsUI;
    [SerializeField] private Text unlocksUI;

    [SerializeField] private Image jumpsImage;
    [SerializeField] private Sprite jumpsOn;
    [SerializeField] private Sprite jumpsOff;
    [SerializeField] private Image locksImage;
    [SerializeField] private Sprite locksOn;
    [SerializeField] private Sprite locksOff;


    public void UpdateValues(int id, int value)
    {
        Text ui;
        switch (id)
        {
            case 2: ui = movesUI; break;
            case 3: ui = jumpsUI; CheckValue(value, true); break;
            case 4: ui = unlocksUI; CheckValue(value, false); break;
            default: ui = globalScoreUI; break;
        }

        UpdateText(ui, value.ToString());
    }

    private void CheckValue(int value, bool jumps)
    {
        Image targetImage = jumps ? jumpsImage : locksImage;
        if(value <= 0)
        {
            targetImage.sprite = jumps ? jumpsOff : locksOff;
        }
        else
        {
            targetImage.sprite = jumps ? jumpsOn : locksOn;
        }
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f).Play();
    }
}
