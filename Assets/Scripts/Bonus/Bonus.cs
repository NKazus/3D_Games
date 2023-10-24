using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Bonus : MonoBehaviour
{
    [SerializeField] private BonusSlot[] slots;
    [SerializeField] private CageHighlight visuals;
    [SerializeField] private Sprite bonusIcon;

    [SerializeField] private Text gameSessionText;
    [SerializeField] private Text clicksRemainedText;
    [SerializeField] private Text hintText;
    [SerializeField] private string hintDefault;
    [SerializeField] private string hintGameSession;
    [SerializeField] private string hintReward;

    [SerializeField] private int maxGameSessions;

    [SerializeField] private int maxClickCount = 3;
    [SerializeField] private int maxAwardCount = 1;
    [SerializeField] private int winValue = 3;

    private int clickCount;
    private int awardCount;
    private bool isEnabled = false;

    [Inject] private readonly ResourceController resources;
    [Inject] private readonly Randomizer rand;

    private void Awake()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].Initialize();
        }
    }

    private void OnEnable()
    {
        visuals.SetHighlight(Highlight.Active);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Refresh();
        }
        CheckClicks();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetInteractable(isEnabled);
            slots[i].SetBaseColor(isEnabled);
        }
        if (isEnabled)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].SetListener(Click);
            }
        }
        hintText.text = isEnabled ? hintDefault : hintGameSession;
        clickCount = isEnabled ? maxClickCount : 0;
        clicksRemainedText.text = clickCount.ToString();
    }

    private void OnDisable()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ResetListener();
        }
    }

    private void Click(BonusSlot target)
    {
        if (!isEnabled)
        {
            return;
        }
        if (clickCount <= 0)
        {
            visuals.SetHighlight(Highlight.Failed);
            Complete();
            return;            
        }
        if (awardCount > 0)
        {
            if (RandomizeReward())
            {
                hintText.text = hintReward;
                resources.UpdateScanExtra(winValue);

                visuals.SetHighlight(Highlight.Normal);
                target.SetRewardIcon(bonusIcon);
                awardCount--;
                Complete();
            }
        }
        clickCount--;
        clicksRemainedText.text = clickCount.ToString();
    }

    private bool RandomizeReward()
    {
        return rand.GetInt(0, 2) == 1;
    }

    private void Complete()
    {
        isEnabled = false;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetInteractable(isEnabled);
        }
        resources.UpdateGameSession(true); //reset
        gameSessionText.text = resources.CurrentGameSession.ToString() + "/" + maxGameSessions.ToString();
    }

    private void CheckClicks()
    {
        int gameSession = resources.CurrentGameSession;

        if(gameSession < maxGameSessions)
        {
            clickCount = 0;
            awardCount = 0;
            isEnabled = false;
        }
        else
        {
            if(gameSession > maxGameSessions)
            {
                gameSession = maxGameSessions;
            }
            clickCount = maxClickCount - 1;
            awardCount = maxAwardCount;
            isEnabled = true;
        }
        gameSessionText.text = gameSession.ToString() + "/" + maxGameSessions.ToString();
    }
}
