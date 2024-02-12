using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MessageSystem : MonoBehaviour
{
    [SerializeField] private GameObject noLampPanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private Text messageText;
    [SerializeField] private string winText;
    [SerializeField] private string loseText;
    [SerializeField] private Image resultImage;
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite loseSprite;

    private Transform noLampTransform;
    private Transform hintTransform;

    private void OnDisable()
    {
        DOTween.Kill("messages");
    }

    public void Initialize()
    {
        noLampTransform = noLampPanel.transform;
        hintTransform = hintPanel.transform;
    }

    public void ResetMessages()
    {
        noLampPanel.SetActive(false);
        hintPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    public void ShowLampMessage()
    {
        resultPanel.SetActive(false);
        noLampTransform.localScale = Vector3.zero;
        noLampPanel.SetActive(true);
        noLampTransform.DOScale(new Vector3(1, 1, 1), 0.5f)
            .SetId("messages");
    }

    public void ShowHint()
    {
        resultPanel.SetActive(false);
        hintTransform.localScale = Vector3.zero;
        hintPanel.SetActive(true);
        hintTransform.DOScale(new Vector3(1, 1, 1), 0.5f)
            .SetId("messages");
    }

    public void ShowResult(bool win)
    {
        resultImage.sprite = win ? winSprite : loseSprite;
        messageText.text = win ? winText : loseText;
        hintTransform.DOScale(Vector3.zero, 0.5f)
            .SetId("messages")
            .OnComplete(() =>
            {
                hintPanel.SetActive(false);
                resultPanel.SetActive(true);
            });
    }
}
