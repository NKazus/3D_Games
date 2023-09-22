using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ColorRange : MonoBehaviour
{
    [SerializeField] private CardHandler card;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Button option1;
    [SerializeField] private Button option2;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private string win;
    [SerializeField] private string lose;
    [SerializeField] private Sprite winIcon;
    [SerializeField] private Sprite loseIcon;

    private RangeOption range1;
    private RangeOption range2;

    private Text rewardText;
    private Image rewardImage;

    private float currentHSVHue;

    [Inject] private readonly Resources resources;
    [Inject] private readonly EventManager events;
    [Inject] private readonly Randomizer randomizer;

    private void Awake()
    {
        range1 = option1.transform.GetChild(0).GetComponent<RangeOption>();
        range2 = option2.transform.GetChild(0).GetComponent<RangeOption>();

        range1.InitRange();
        range2.InitRange();

        rewardImage = rewardPanel.transform.GetChild(1).GetComponent<Image>();
        rewardText = rewardPanel.transform.GetChild(2).GetComponent<Text>();
    }

    private void OnEnable()
    {
        resources.UpdatePlayerScore(0);
        resources.UpdateInspiration(0);

        optionPanel.SetActive(false);
        rewardPanel.SetActive(false);

        if (resources.PlayerScore > 0)
        {
            scoreText.enabled = false;
            Invoke("GenerateCard", 1f);
        }
        else
        {
            ShowScoreText();
        }
    }

    private void OnDisable()
    {
        if (IsInvoking())
        {
            CancelInvoke("GenerateCard");
        }
        option1.onClick.RemoveAllListeners();
        option2.onClick.RemoveAllListeners();
    }

    private void GenerateCard()
    {
        rewardPanel.SetActive(false);
        currentHSVHue = randomizer.GenerateFloat(0f, 1f);
        card.Activate(Color.HSVToRGB(currentHSVHue, 1f, 1f), CardCallback);
    }

    private void CardCallback()
    {
        bool firstRight = randomizer.GenerateInt(0, 10) > 5;
        float axisDir = randomizer.GenerateInt(0, 10) > 5 ? 1f : -1f;

        Color colorTop = Color.HSVToRGB((currentHSVHue + axisDir * (firstRight ? 0.1f : 0.15f)) % 1f, 1f, 1f);
        Color colorBot = Color.HSVToRGB((currentHSVHue + axisDir * 0.25f) % 1f, 1f, 1f);
        range1.SetRange(colorTop, colorBot);

        axisDir = -axisDir;
        colorTop = Color.HSVToRGB((currentHSVHue + axisDir * (firstRight ? 0.15f : 0.1f)) % 1f, 1f, 1f);
        colorBot = Color.HSVToRGB((currentHSVHue + axisDir * 0.25f) % 1f, 1f, 1f);
        range2.SetRange(colorTop, colorBot);

        option1.onClick.AddListener(() => ChooseOption(firstRight));
        option2.onClick.AddListener(() => ChooseOption(!firstRight));
        optionPanel.SetActive(true);
    }

    private void ChooseOption(bool correct)
    {
        option1.onClick.RemoveAllListeners();
        option2.onClick.RemoveAllListeners();
        optionPanel.SetActive(false);

        resources.UpdatePlayerScore(-1);
        if (correct)
        {
            resources.UpdateInspiration(1);
            events.PlayCharge();
            rewardText.text = win;
            rewardImage.sprite = winIcon;            
        }
        else
        {
            events.PlayVibro();
            rewardText.text = lose;
            rewardImage.sprite = loseIcon;
        }
        rewardPanel.SetActive(true);

        if (resources.PlayerScore > 0)
        {
            Invoke("GenerateCard", 1f);
        }
        else
        {
            Invoke("ShowScoreText", 1f);
        }
    }

    private void ShowScoreText()
    {
        rewardPanel.SetActive(false);
        scoreText.enabled = true;
    }

}
